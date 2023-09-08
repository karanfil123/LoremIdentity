using LoremIdentity.Web.Extensions;
using LoremIdentity.Web.Models;
using LoremIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace LoremIdentity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> usermanager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _usermanager = usermanager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _usermanager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel
            {
                Username = currentUser!.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
            return View(userViewModel);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var checkOldpasseowrd = await _usermanager.CheckPasswordAsync(currentUser, model.PasswordOld);
            if (!checkOldpasseowrd)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış.");
                return View();
            }
            var resultChangePassword = await _usermanager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordNew);
            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelStateErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await _usermanager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, model.PasswordNew, true, false);
            TempData["SuccededMessage"] = @$"Şifre değiştirildi.";
            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));

            var currenUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var userEditvİewModel = new UserEditViewModel
            {
                Username = currenUser.UserName,
                Email = currenUser.Email,
                Phone = currenUser.PhoneNumber,
                BirthDate = currenUser.BirthDate,
                City = currenUser.City,
                Gender = currenUser.Gender,
            };
            return View(userEditvİewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            currentUser.UserName = request.Username;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;
            if (request.Picture != null && request.Picture.Length > 0)
            {
                // wwwroot dizininde gezinmek için dosya sağlayıcısı alınıyor
                var wwwroot = _fileProvider.GetDirectoryContents("wwwroot");

                // Rastgele bir dosya adı oluşturuluyor
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                // Yeni resim dosyasının yolu belirleniyor
                var newPicturePath = Path.Combine(wwwroot.First(x => x.Name == "userpictures").PhysicalPath, randomFileName);

                // Dosya akışı oluşturuluyor ve resim dosyası buraya kopyalanıyor
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;
            }
            var updateTouserResult = await _usermanager.UpdateAsync(currentUser);
            if (!updateTouserResult.Succeeded)
            {
                ModelState.AddModelStateErrorList(updateTouserResult.Errors);

                return View();
            }
            await _usermanager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);
            TempData["SuccededMessage"] = @$"Kullanıcı bilgileri değiştirildi.";
            return View(request);
        }

        public async Task<IActionResult> AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            ViewBag.message = "Bu sayfaya erişim hakkınız yoktur yöneticinizle iletişime geçiniz.";
            return View();
        }

        public async Task<IActionResult> Claims()
        {
            var userClaimList = User.Claims.Select(x => new ClaimViewModel
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();
            return View(userClaimList);
        }
    }
}