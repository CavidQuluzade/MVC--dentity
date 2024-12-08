using İdentity.Constants;
using İdentity.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace İdentity
{
    public static class DbInitializer
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            AddRoles(roleManager);
            AddAdmin(userManager, roleManager);
        }
        private static void AddRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues<UserRoles>())
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    _ = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    }).Result;
                }
            }
        }
        private static void AddAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (userManager.FindByEmailAsync("admin@admin.com").Result is null)
            {
                var user = new User
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    City = "Baku",
                    Country = "Azerbaijan"
                };
                var result = userManager.CreateAsync(user, "Admin123@").Result;
                if (!result.Succeeded)
                    throw new Exception("Admin can't be created");

                var role = roleManager.FindByNameAsync("Admin").Result;
                if (role?.Name == null)
                {
                    throw new Exception("Admin role doesn't exists");
                }
                var addRoleToUser = userManager.AddToRoleAsync(user, role.Name).Result;
                if (!addRoleToUser.Succeeded)
                    throw new Exception("Cannot assign admin role to user");
            }
        }
    }
}
