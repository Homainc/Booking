using System.Threading.Tasks;
using Booking.Models;
using Booking.Services;
using Booking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email) => 
            Json((await _userManager.FindByEmailAsync(email)) == null);

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if(!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }
                var result = 
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Surname = model.Surname
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId= user.Id, code = code},
                        protocol: HttpContext.Request.Scheme);
                    await _emailService.SendEmailAsync(model.Email, "Подтверждение регистрации",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
                    var msg = $"На вашу почту {model.Email} было отправлено сообщение. Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме.";
                    return View("Info", new InfoViewModel { Title = "Подтверждение адреса электронной почты", Message = msg });
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                var msg = $"Поздравляем! Регистрация прошла успешна. Адрес {user.Email} подтверждён.";
                return View("Info", new InfoViewModel { Title = "Завершение регистрации", Message = msg });
            }
            else
                return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            _userManager.Dispose();
            base.Dispose(disposing);
        }
    }
}