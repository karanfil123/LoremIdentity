using LoremIdentity.Web.Areas.Admin.Models;
using LoremIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoremIdentity.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace LoremIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleListViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return View(roles);
        }

        public IActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleAdd(RoleAddViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole { Name = request.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelStateErrorList(result.Errors);
                return View();
            }
            return RedirectToAction(nameof(RoleController.Index));
        }

        public async Task<IActionResult> RoleUpdate(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Güncellemek istediğiniz rol bulunamadı.");

            return View(new RoleUpdateViewModel { Id = role.Id, Name = role.Name });
        }

        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            var result = await _roleManager.FindByIdAsync(request.Id);
            if (result == null)
                throw new Exception("Güncellemek istediğiniz rol bulunamadı.");
            result.Name = request.Name;
            await _roleManager.UpdateAsync(result);
            TempData["SuccededMessage"] = @$"Rol güncellendi.";
            return RedirectToAction(nameof(RoleController.Index));
        }

        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Güncellemek istediğiniz rol bulunamadı.");

            var resultRole = await _roleManager.DeleteAsync(role);
            if (!resultRole.Succeeded)
                throw new Exception(resultRole.Errors.Select(x => x.Description).First());
            return RedirectToAction(nameof(RoleController.Index));
        }

        public async Task<IActionResult> RoleToUserAssing(string id)
        {
            ViewBag.UserID = id;
            var currentUser = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var roleviewModel = new List<RoleAssingUserViewModel>();

            var userRoles = await _userManager.GetRolesAsync(currentUser);
            foreach (var role in roles)
            {
                var assingRoleToruser = new RoleAssingUserViewModel() { Id = role.Id, Name = role.Name };
                if (userRoles.Contains(role.Name))
                {
                    assingRoleToruser.Exist = true;
                }
                roleviewModel.Add(assingRoleToruser);
            }
            return View(roleviewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RoleToUserAssing(string id, List<RoleAssingUserViewModel> roles)
        {
            var userToassingRoles = await _userManager.FindByIdAsync(id);
            foreach (var role in roles)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToassingRoles, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToassingRoles, role.Name);

                }
            }
            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }
    }
}