using İdentity.Data.Entities;
using İdentity.Utilities.EmailHandler.Abstract;
using İdentity.Utilities.EmailHandler.Concrete;
using İdentity.Utilities.EmailHandler.Models;
using İdentity.ViewModels.Account;
using İdentity.ViewModels.Subscription;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace İdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Register(AccountRegisterVM registerModel)
        {
            if (!ModelState.IsValid) return View(registerModel);
            var user = new User
            {
                Email = registerModel.EmailAddress,
                UserName = registerModel.EmailAddress,
                City = registerModel.City,
                Country = registerModel.Country
            };
            var result = _userManager.CreateAsync(user, registerModel.Password).Result;
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(registerModel);
            }

            var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            var url = Url.Action(nameof(ConfirmEmail), "Account", new {token, user.Email}, Request.Scheme);
            _emailService.SendEmail(new Utilities.EmailHandler.Models.Message(new List<string> { user.Email }, "Email COnfirmation", url));
            return RedirectToAction(nameof(Login));
        }
        
        public IActionResult ConfirmEmail(string email, string token)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if(user == null) return NotFound();
            var result = _userManager.ConfirmEmailAsync(user, token).Result;
            if (!result.Succeeded) return NotFound();
            return RedirectToAction(nameof(Login));
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
            var result = _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false).Result;
            if (!result.Succeeded) 
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View(loginModel);
            }
            var confirmResult = _userManager.IsEmailConfirmedAsync(user).Result;
            if (!confirmResult)
            {
                ModelState.AddModelError(string.Empty, "Email doesn't confirmed");
                return View(loginModel);
            }
            if (!string.IsNullOrEmpty(loginModel.returnURL) && Url.IsLocalUrl(loginModel.returnURL)) return Redirect(loginModel.returnURL);
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult ForgotPassword(AccountForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user == null)
            {
                ModelState.AddModelError("Email", "User not found");
                return View(model);
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var url = Url.Action(nameof(ResetPassword), "Account", new { token, user.Email }, Request.Scheme);
            _emailService.SendEmail(new Utilities.EmailHandler.Models.Message(new List<string> { user.Email}, "Forgot Password?", url));

            ViewBag.NotificationText = "Mail has been send. Check your email box";
            return View("Notification");
        }
        
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(AccountResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByNameAsync(model.EmailAddress).Result;
            if (user == null)
            {
                ModelState.AddModelError("EmailAdress", "User not found");
                return View(model);
            }
            var result = _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult Subscribe(SubscribeVM model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Home");
            var result = _userManager.FindByEmailAsync(model.EmailAddress).Result;
            if (result == null)
            {
                ModelState.AddModelError("EmailAdress", "Email must be registered");
                return RedirectToAction("Index", "Home");
            }
            result.isSubscribed = true;
            var isUpdated = _userManager.UpdateAsync(result).Result;
            if (!isUpdated.Succeeded)
            {
                ModelState.AddModelError("EmailAdress", "Error occured. TRy again");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
