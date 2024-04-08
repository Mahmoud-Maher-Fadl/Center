using Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Presentation
{
    public static class AppExtensions
    {
        public static async Task RoleSeeds(this IHost webApplication)
        {
            using var scope = webApplication.Services.CreateScope();  // Remember to study the using statement
            var serviceProvider = scope.ServiceProvider;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Center", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            const string adminUserName = "admin@centers.com";
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUserName,
                    Email = adminUserName
                };
                await userManager.CreateAsync(adminUser, "Admin@2019");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}