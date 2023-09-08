using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LoremIdentity.Web.TagHelpers
{
    public class UserPictureTagHelper : TagHelper
    {
        public string PictureuUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (string.IsNullOrEmpty(PictureuUrl))
            {
                output.Attributes.SetAttribute("src", "/userpictures/defaultuser.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureuUrl}");
            }
            base.Process(context, output);
        }
    }
}