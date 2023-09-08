using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace LoremIdentity.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isDigit = int.TryParse(user.UserName[0].ToString(), out _);

            if (isDigit)
            {
                errors.Add(new()
                {
                    Code = "UserNameContainFirstletterDigit",
                    Description = "Kullanıcı adı sayısal bir karekterle başlayamaz"
                });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            };
            return Task.FromResult(IdentityResult.Success);
        }
    }
}