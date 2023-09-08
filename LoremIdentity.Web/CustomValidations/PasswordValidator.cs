using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace LoremIdentity.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();
            if (password!.Contains(user.UserName!.ToLower()))
            {
                errors.Add(new()
                {
                    Code = "PaswordNoContentusername",
                    Description = "Şifre alanı kullanıcı alanı içeremez."
                });
            }
            if (password.ToLower().StartsWith("1234"))
            {
                errors.Add(new()
                {
                    Code = "PaswordNoContain1234",
                    Description = "Şifre alanı ardışık sayılarla başlayamaz"
                });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}