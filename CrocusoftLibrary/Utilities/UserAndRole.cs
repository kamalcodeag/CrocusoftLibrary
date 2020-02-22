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
            #region Create roles("Admin", "İstifadəçi") and main user("admin")
            //Check if the "Admin" role exists in db or not. If not existed, create a new one.
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Check if the "İstifadəçi" role exists in db or not. If not existed, create a new one.
            if (!await _roleManager.RoleExistsAsync("İstifadəçi"))
            {
                await _roleManager.CreateAsync(new IdentityRole("İstifadəçi"));
            }

            //Find a user called "admin" in db
            CustomUser customUserFromDb = await _userManager.FindByNameAsync("admin");
            //If the user does not exist, create a new one
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
            #region Define a user is in "Admin" role or not
            //Find a username passed by parameter in db
            CustomUser customUserFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            //Find the user's role(s)
            IList<string> roles = await _userManager.GetRolesAsync(customUserFromDb);

            if (roles.ElementAt(0) != "Admin")
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
