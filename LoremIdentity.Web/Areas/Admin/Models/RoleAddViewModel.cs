using System.ComponentModel.DataAnnotations;

namespace LoremIdentity.Web.Areas.Admin.Models
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage = "Rol alanı boş geçilemez.")]
        [Display(Name = "Rol Adı*")]
        public string Name { get; set; }
    }
}
