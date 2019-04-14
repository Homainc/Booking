using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Services
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(
            IConfiguration configuration, 
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var adminConfig = configuration.GetSection("AdminAccount");
            var adminEmail = adminConfig.GetValue<string>("Email");
            var adminPassword = adminConfig.GetValue<string>("Password");
            var adminName = adminConfig.GetValue("Name", "");
            var adminSurname = adminConfig.GetValue("Surname", "");

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));
            if (await roleManager.FindByNameAsync("manager") == null)
                await roleManager.CreateAsync(new IdentityRole("manager"));
            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Name = adminName,
                    Surname = adminSurname,
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
