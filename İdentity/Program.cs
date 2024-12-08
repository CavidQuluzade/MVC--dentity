using İdentity.Data;
using İdentity.Data.Entities;
using İdentity.Utilities.EmailHandler.Abstract;
using İdentity.Utilities.EmailHandler.Concrete;
using İdentity.Utilities.EmailHandler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace İdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<IEmailService, EmailService>();    

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            var emailConfiguration = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfiguration);
            var app = builder.Build();
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}"
                );

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                DbInitializer.SeedData(userManager, roleManager);
            }
            app.Run();
        }
    }
}
