using Microsoft.AspNetCore.Identity;

namespace LoremIdentity.Web.CustomValidations
{
    public class LocalIdentityDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "Duplicateusername",
                Description = $"{userName} kullanıcı adı daha önceden alınmıştır."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "Duplicateusername",
                Description = $"{email} bu email daha önceden kullanılmış."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "Duplicateusername",
                Description = "Şifre en az 3 karekter olmalıdır."
            };
        }
    }
}