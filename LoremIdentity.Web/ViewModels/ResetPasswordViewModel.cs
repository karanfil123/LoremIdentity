using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Şife tekrar alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre Tekrar*")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}