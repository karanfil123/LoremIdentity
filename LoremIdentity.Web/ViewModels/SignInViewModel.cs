using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
        }

        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [Display(Name = "Şifre*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}