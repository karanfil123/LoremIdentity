using LoremIdentity.Web.Areas.Admin.Models;
using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoremIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,editör")]

    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userviewModelsList = users.Select(x => new UserViewModel
            {
                Id = x.Id,
                Name = x.UserName,
                Email = x.Email,
                City = x.City
            }).ToList();
            return View(userviewModelsList);
        }
    }
}