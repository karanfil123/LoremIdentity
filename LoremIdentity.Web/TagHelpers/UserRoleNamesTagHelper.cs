using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace LoremIdentity.Web.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; }
        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var userroles = await _userManager.GetRolesAsync(user);
            var stringbuilder = new StringBuilder();

            userroles.ToList().ForEach(role =>
            {
                stringbuilder.Append(@$"<span class='badge bg-danger mx-1'>{role.ToLower()}</span>");
            });
            output.Content.SetHtmlContent(stringbuilder.ToString());
        }
    }
}
