using LoremIdentity.Web.Extensions;
using LoremIdentity.Web.MailServices;
using LoremIdentity.Web.Models;
using LoremIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoremIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel we)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = we.Username,
                Email = we.Email,
                City = we.City,
                PhoneNumber = we.Phone
            }, we.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["SuccededMessage"] = "Kayıt işlemi başarılı.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelStateErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel we, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            returnUrl = returnUrl ?? Url.Action(nameof(HomeController.Index));

            var hasuser = await _userManager.FindByEmailAsync(we.Email);
            if (hasuser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre hatalı kontrol edip tekrar deneyiniz.");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(hasuser, we.Password, we.RememberMe, true);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelStateErrorList(new List<string>() { "3 kez hatalı giriş yaptınız 3 dakika sonra tekrar deneyiniz." });

                return View();
            }
            var accesfailedCount = await _userManager.GetAccessFailedCountAsync(hasuser);

            ModelState.AddModelStateErrorList(new List<string>() { $"Email veya Şifre hatalı kontrol edip tekrar deneyiniz.",
                $"Başarısız giriş sayısı:{accesfailedCount}" });

            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunamadı.");
                return View();
            }
            string passwordToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordToken }, HttpContext.Request.Scheme);

            await _mailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

            TempData["SuccededMessage"] = @$"Şifre yenileme linki ({hasUser.Email}) adresine gönderilmiştilr.";

            return RedirectToAction(nameof(HomeController.ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir Hata oluştu.");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString());

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token.ToString(), model.Password);

            if (result.Succeeded)
            {
                TempData["SuccededMessage"] = "Şİfrenin başarılı şekilde yenilendi";
            }
            else
            {
                ModelState.AddModelStateErrorList(result.Errors.Select(x => x.Description).ToList());
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}