using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LoremIdentity.Web.Extensions
{
    public static class ModelStateExt
    {
        public static void AddModelStateErrorList(this ModelStateDictionary modelstate, List<string> errors)
        {
            errors.ForEach(x =>
            {
                modelstate.AddModelError(string.Empty, x);
            });
        }

        public static void AddModelStateErrorList(this ModelStateDictionary modelstate, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(x =>
            {
                modelstate.AddModelError(string.Empty, x.Description);
            });
        }
    }
}