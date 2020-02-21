using CrocusoftLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.Utilities
{
    public static class UserAndRole
    {
        public static async Task<bool> DbInitializer(UserManager<CustomUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            #region Creating Role("Admin") and main user("admin") while running app
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("İstifadəçi"))
            {
                await _roleManager.CreateAsync(new IdentityRole("İstifadəçi"));
            }

            CustomUser customUserFromDb = await _userManager.FindByNameAsync("admin");
            if (customUserFromDb == null)
            {
                CustomUser customUser = new CustomUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Age = 25,
                    UserName = "admin"
                };

                IdentityResult result = await _userManager.CreateAsync(customUser, "1234cladmin2020");

                if (!result.Succeeded)
                {
                    return false;
                }

                await _userManager.AddToRoleAsync(customUser, "Admin");
            }
            #endregion

            return true;
        }

        public static async Task<bool> IsAdminRole(UserManager<CustomUser> _userManager, string username)
        {
            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            IList<string> roles =  await _userManager.GetRolesAsync(customUserFromDb);

            if(roles.ElementAt(0) != "Admin")
            {
                return false;
            }

            return true;
        }
    }
}
