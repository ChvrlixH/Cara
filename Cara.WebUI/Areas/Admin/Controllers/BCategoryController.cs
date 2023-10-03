using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class BCategoryController : Controller
	{
		private IBCategoryRepository _repository;

		public BCategoryController(IBCategoryRepository repository)
		{
			_repository = repository;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _repository.ListAsync();
			return View(categories);
		}

		public async Task<IActionResult> Detail(int id)
		{
			var category = await _repository.FirstInclude(id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		public async Task<IActionResult> Update(int? id)
		{
			if (id == null) { return NotFound(); }

			BCategory category = await _repository.GetAsync(id);
			if (category == null) { return NotFound(); }
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int? id, BCategory editedCategory)
		{
			if (id == null) { return NotFound(); }
			if (!ModelState.IsValid) return View(editedCategory);
			BCategory category = await _repository.GetAsync(id);
			if (category == null) { return NotFound(); }

			bool isExist = _repository.AnyAsync(editedCategory);
			if (isExist)
			{
				ModelState.AddModelError("Name", "Name already exist");
				return View();
			}

			category.Name = editedCategory.Name;

			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BCategory category)
		{
			if (!ModelState.IsValid)
			{
				return View(category);
			}

			bool isExist = _repository.AnyAsync(category);
			if (isExist)
			{
				ModelState.AddModelError("Name", "Name already exist");
				return View();
			}

			BCategory newCategory = new BCategory
			{
				Name = category.Name
			};

			_repository.CreateAsync(newCategory);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return NotFound(); }
			BCategory category = await _repository.GetAsync(id);
			if (category == null) { return NotFound(); }
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeletePost(int? id)
		{
			if (id == null) { return NotFound(); }
			BCategory category = await _repository.GetAsync(id);
			if (category == null) { return NotFound(); }

			category.IsDeleted = true;

			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
