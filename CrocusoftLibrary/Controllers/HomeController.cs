using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrocusoftLibrary.DAL;
using CrocusoftLibrary.Models;
using CrocusoftLibrary.Utilities;
using CrocusoftLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrocusoftLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrocusoftLibraryDb _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<CustomUser> _signInManager;


        public HomeController(CrocusoftLibraryDb context,
                              UserManager<CustomUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<CustomUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Login()
        {
            bool succeeded = await UserAndRole.DbInitializer(_userManager, _roleManager);

            if (succeeded == false)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Employee");
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            CustomUser customUserFromDb = await _userManager.FindByNameAsync(loginVM.UserName);

            if (customUserFromDb == null)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz istifadəçi adı və ya şifrə yanlışdır.");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(customUserFromDb, loginVM.Password, loginVM.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz istifadəçi adı və ya şifrə yanlışdır.");
                return View(loginVM);
            }

            return RedirectToAction("List", "Employee");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Employee");
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
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

            TempData["Registered"] = true;
            return RedirectToAction("Login", "Home");
        }
    }
}