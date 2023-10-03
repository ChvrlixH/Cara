using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class TagController : Controller
	{
		private ITagRepository _repository;

		public TagController(ITagRepository repository)
		{
			_repository = repository;
		}

		public async Task<IActionResult> Index()
		{
			var tags = await _repository.ListAsync();
			return View(tags);
		}

		public async Task<IActionResult> Detail(int id)
		{
			var tag = await _repository.FirstThenInclude(id);

			if (tag == null)
			{
				return NotFound();
			}

			return View(tag);
		}

		public async Task<IActionResult> Update(int? id)
		{
			if (id == null) { return NotFound(); }

			Tag tag = await _repository.GetAsync(id);
			if (tag == null) { return NotFound(); }
			return View(tag);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int? id, Tag editedtag)
		{
			if (id == null) { return NotFound(); }
			if (!ModelState.IsValid) return View(editedtag);
			Tag tag = await _repository.GetAsync(id);
			if (tag == null) { return NotFound(); }

			bool isExist = _repository.AnyAsync(editedtag);
			if (isExist)
			{
				ModelState.AddModelError("Name", "Name already exist");
				return View();
			}

			tag.Name = editedtag.Name;

			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Tag tag)
		{
			if (!ModelState.IsValid)
			{
				return View(tag);
			}

			bool isExist = _repository.AnyAsync(tag);
			if (isExist)
			{
				ModelState.AddModelError("Name", "Name already exist");
				return View();
			}

			Tag newtag = new Tag
			{
				Name = tag.Name
			};

			_repository.CreateAsync(newtag);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) { return NotFound(); }
			Tag tag = await _repository.GetAsync(id);
			if (tag == null) { return NotFound(); }
			return View(tag);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeletePost(int? id)
		{
			if (id == null) { return NotFound(); }
			Tag tag = await _repository.GetAsync(id);
			if (tag == null) { return NotFound(); }

			_repository.Delete(tag);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
