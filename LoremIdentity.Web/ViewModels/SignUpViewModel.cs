using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
        }

        public SignUpViewModel(string username, string email, string phone, string password)
        {
            Username = username;
            Email = email;
            Phone = phone;
            Password = password;
        }

        [Required(ErrorMessage = "Kullanıcı alanı boş geçilemez.")]
        [Display(Name = "Kullanıcı Adı*")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Eposta boş geçilemez.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "E-Posta*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şehir alanı boş geçilemez.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Telefon alanı boş geçilemez.")]
        [Display(Name = "Telefon*")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [Display(Name = "Şifre*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "şife tekrar alanı boş geçilemez.")]
        [Display(Name = "Şifre Tekrar*")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}