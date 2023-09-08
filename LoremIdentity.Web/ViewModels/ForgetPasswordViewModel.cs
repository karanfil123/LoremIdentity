using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email alanı boş geçilemez.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "Email*")]
        public string Email { get; set; }
    }
}