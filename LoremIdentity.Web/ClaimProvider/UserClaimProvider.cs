using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LoremIdentity.Web.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            var currentUser = await _userManager.FindByNameAsync(identity.Name);
            if (currentUser == null)
            {
                return principal;
            }
            if (currentUser.City == null)
            {
                return principal;
            }
            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);
                identity.AddClaim(cityClaim);
            }
            return principal;

        }
    }
}
