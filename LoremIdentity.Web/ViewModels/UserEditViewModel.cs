using LoremIdentity.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı alanı boş geçilemez.")]
        [Display(Name = "Kullanıcı Adı*")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Eposta boş geçilemez.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "E-Posta*")]
        public string Email { get; set; }

        [Display(Name = "Şehir*")]
        public string City { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefon*")]
        public string Phone { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Fotoğraf*")]
        public IFormFile Picture { get; set; }

        [Display(Name = "Cinsiyet*")]
        public Gender Gender { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Duğum Yılı alanı boş geçilemez.")]
        [Display(Name = "Doğum Yılı*")]
        public DateTime BirthDate { get; set; }
    }
}