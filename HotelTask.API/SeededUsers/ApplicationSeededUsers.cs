using HotelTask.API.Authorizations;
using HotelTask.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.SeededUsers
{
    public class ApplicationSeededUsers
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Guest.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Super_Administrator.ToString()));
            //Seed Default User
            var guestUser = new ApplicationUser { UserName = Authorization.default_Guestusername, Email = Authorization.default_Guestemail, EmailConfirmed = true, PhoneNumberConfirmed = true };
            var AdminUser = new ApplicationUser { UserName = Authorization.default_Adminusername, Email = Authorization.default_Adminemail, EmailConfirmed = true, PhoneNumberConfirmed = true };
            var SuperAdminUser = new ApplicationUser { UserName = Authorization.default_SuperAdminUserName, Email = Authorization.default_SuperAdminemail, EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Users.All(u => (u.Id != guestUser.Id && u.Id != AdminUser.Id && u.Id != SuperAdminUser.Id)))
            {
                await userManager.CreateAsync(guestUser, Authorization.default_Guestpassword);
                await userManager.AddToRoleAsync(guestUser, Authorization.default_Guestrole.ToString());

                await userManager.CreateAsync(AdminUser, Authorization.default_Adminpassword);
                await userManager.AddToRoleAsync(AdminUser, Authorization.default_Adminrole.ToString());

                await userManager.CreateAsync(SuperAdminUser, Authorization.default_SuperAdminpassword);
                await userManager.AddToRoleAsync(SuperAdminUser, Authorization.default_SuperAdminrole.ToString());
            }
        }
    }
}
