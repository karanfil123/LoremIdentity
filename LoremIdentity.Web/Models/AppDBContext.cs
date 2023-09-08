using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoremIdentity.Web.Models
{
    public class AppDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
    }
}