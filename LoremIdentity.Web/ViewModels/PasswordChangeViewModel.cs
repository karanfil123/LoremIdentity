using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Mevcut alanı boş geçilemez.")]
        [Display(Name = "Mevcut Şifre*")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimun 6 karekter olmalıdır.")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre*")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimun 6 karekter olmalıdır.")]
        public string PasswordNew { get; set; }

        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler uyuşmuyor.")]
        [Required(ErrorMessage = "Yeni şife tekrar alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre Tekrar*")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimun 6 karekter olmalıdır.")]
        public string PasswordNewConfirm { get; set; }
    }
}