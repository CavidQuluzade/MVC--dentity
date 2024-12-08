using İdentity.Data.Entities;
using İdentity.Areas.Admin.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace İdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AccountLoginVM loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);

            var user = _userManager.FindByEmailAsync(loginModel.EmailAddress).Result;
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View(loginModel);
            }
            if (!_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View(loginModel);
            }

            var result = _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false).Result;
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View(loginModel);
            }
            if (!string.IsNullOrEmpty(loginModel.returnURL) && Url.IsLocalUrl(loginModel.returnURL)) return Redirect(loginModel.returnURL);
            return RedirectToAction("index", "home");
        }
    }
}
