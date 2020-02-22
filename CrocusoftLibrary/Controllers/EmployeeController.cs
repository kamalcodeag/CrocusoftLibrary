using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrocusoftLibrary.DAL;
using CrocusoftLibrary.Models;
using CrocusoftLibrary.Utilities;
using CrocusoftLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrocusoftLibrary.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CrocusoftLibraryDb _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        public EmployeeController(CrocusoftLibraryDb context,
                                  UserManager<CustomUser> userManager,
                                  SignInManager<CustomUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            CustomUser customUser = new CustomUser
            {
                FirstName = registerVM.FirstName.Trim(),
                LastName = registerVM.LastName.Trim(),
                Age = registerVM.Age,
                UserName = registerVM.UserName.Trim()
            };

            IdentityResult result = await _userManager.CreateAsync(customUser, registerVM.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Bu istifadəçi artıq qeydiyyatdan keçib və ya şifrə tələblərə uyğun deyil.");
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(customUser, "İstifadəçi");
            await _context.SaveChangesAsync();

            TempData["Created"] = true;
            return RedirectToAction("List", "Employee");
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            //Define an active user is in "Admin" role or not
            bool isAdminRole = await UserAndRole.IsAdminRole(_userManager, User.Identity.Name);

            if(isAdminRole == true)
            {
                ViewBag.IsAdminRole = 1;
            }

            ViewBag.ActiveUser = User.Identity.Name;

            IEnumerable<CustomUser> customUsers = _userManager.Users.ToList();

            return View(customUsers);
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (customUserFromDb == null)
            {
                return NotFound();
            }

            return View(customUserFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, CustomUser customUser)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (customUserFromDb == null)
            {
                return NotFound();
            }

            //Find an active username
            string activeUserName = User.Identity.Name;
            //Find a username to be updated
            string userNameToBeUpdated = customUserFromDb.UserName;

            customUserFromDb.FirstName = customUser.FirstName;
            customUserFromDb.LastName = customUser.LastName;
            customUserFromDb.Age = customUser.Age;
            customUserFromDb.UserName = customUser.UserName;

            IdentityResult result = await _userManager.UpdateAsync(customUserFromDb);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Xəta baş verdi");
                return View(customUser);
            }

            //Check if a username to be updated belongs to a active user
            if(activeUserName == userNameToBeUpdated)
            {
                //Make the user signed in with a new username
                await _signInManager.SignInAsync(customUserFromDb, true);
            }

            TempData["Updated"] = true;
            return RedirectToAction("List", "Employee");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (customUserFromDb == null)
            {
                return NotFound();
            }

            return View(customUserFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (customUserFromDb == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(customUserFromDb);
            await _context.SaveChangesAsync();

            TempData["Deleted"] = true;
            return RedirectToAction("List", "Employee");
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordVM);
            }

            string activeUserName = User.Identity.Name;
            CustomUser customUserFromDb = await _userManager.FindByNameAsync(activeUserName);

            IdentityResult result = await _userManager.ChangePasswordAsync(customUserFromDb, changePasswordVM.CurrentPassword, changePasswordVM.NewPassword);
            await _userManager.UpdateAsync(customUserFromDb);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Hazırki şifrə yanlışdır və ya yeni şifrə tələblərə uyğun deyil.");
                return View(changePasswordVM);
            }

            TempData["PasswordChanged"] = true;
            return RedirectToAction("List", "Employee");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
    }
}