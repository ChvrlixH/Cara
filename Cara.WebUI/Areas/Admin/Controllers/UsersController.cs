using Cara.Core.Entities;
using Cara.WebUI.Areas.Admin.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Cara.WebUI.Utilities.Helper;
using System.Data;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

		public async Task<IActionResult> Index()
		{
			var userName = HttpContext?.User?.Identity?.Name;

			var users = await _userManager.Users.Where(u => u.UserName != userName).ToListAsync();

			List<AllUserVM> allUsers = new List<AllUserVM>();
			foreach (var user in users)
			{
				var userRoles = await _userManager.GetRolesAsync(user);
				allUsers.Add(new AllUserVM
				{
					Id = user.Id,
					Fullname = user.Fullname,
					Email = user.Email,
					Username = user.UserName,
					IsActive = user.IsActive,
					Role = userRoles.FirstOrDefault()
				});

			}

			return View(allUsers);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangeRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) { return NotFound(); }

			var userRoles = await _userManager.GetRolesAsync(user);
			var userRole = userRoles.FirstOrDefault();

			if (userRole == RoleType.Admin.ToString()) { return BadRequest(); }

			UserVM userVM = new()
			{
				Role = userRoles.FirstOrDefault()
			};



			ViewBag.Roles = _roleManager.Roles.ToList();

			return View(userVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangeRole(string id, UserVM userVM)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) { return NotFound(); }

			var userRoles = await _userManager.GetRolesAsync(user);
			var userRole = userRoles.FirstOrDefault();

			if (userRole == RoleType.Admin.ToString())
			{
				return BadRequest();
			}

			await _userManager.RemoveFromRoleAsync(user, userRole);

			await _userManager.AddToRoleAsync(user, userVM.Role);

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Block(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null) { return NotFound(); }

			if (!user.IsActive)
				return BadRequest();

			var userRoles = await _userManager.GetRolesAsync(user);
			var userRole = userRoles.FirstOrDefault();

			if (userRole == RoleType.Admin.ToString())
			{
				return BadRequest();
			}

			user.IsActive = false;
			var Result = await _userManager.UpdateAsync(user);

			return RedirectToAction(nameof(Index));

		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Unblock(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null) { return NotFound(); }

			if (user.IsActive)
				return BadRequest();

			var userRoles = await _userManager.GetRolesAsync(user);
			var userRole = userRoles.FirstOrDefault();

			if (userRole == RoleType.Admin.ToString())
			{
				return BadRequest();
			}

			user.IsActive = true;
			var Result = await _userManager.UpdateAsync(user);

			return RedirectToAction(nameof(Index));
		}
	}
}

