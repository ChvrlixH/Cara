using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.Comment;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class CommentController : Controller
	{
		private readonly ICommentRepository _repository;
		private readonly IWebHostEnvironment _env;
		public CommentController(ICommentRepository repository, IWebHostEnvironment env)
		{
			_repository = repository;
			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _repository.GetAllAsync());
		}

		public async Task<IActionResult> Detail(int id)
		{
			var commentItem = await _repository.GetAsync(id);
			if (commentItem == null) { return NotFound(); }
			return View(commentItem);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CommentVM commentVM)
		{
			if (!ModelState.IsValid) return View(commentVM);
			if (commentVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(commentVM);
			}

			if (!commentVM.Photo.CheckFileSize(100))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
				return View(commentVM);
			}

			if (!commentVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(commentVM);
			}

			var filename = String.Empty;

			try
			{
				filename = await commentVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "comments");
			}
			catch (Exception)
			{

				return View(commentVM);
			}

			Comment commentItem = new()
			{
				Fullname = commentVM.Fullname,
				Profession = commentVM.Profession,
				Description = commentVM.Description,
				Photo = filename
			};

			await _repository.CreateAsync(commentItem);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			var comment = await _repository.GetAsync(id);
			if (comment == null) return NotFound();

			CommentUpdateVM commentUpdateVM = new()
			{
				Fullname = comment.Fullname,
				Profession = comment.Profession,
				Description = comment.Description,
				ImagePath = comment.Photo
			};

			return View(commentUpdateVM);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, CommentUpdateVM commentUpdateVM)
		{
			if (id != commentUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(commentUpdateVM);
			var comment = await _repository.GetAsync(id);
			if (comment == null) return NotFound();

			comment.Fullname = commentUpdateVM.Fullname;
			comment.Profession = commentUpdateVM.Profession;
			comment.Description = commentUpdateVM.Description;
			if (commentUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "comments", comment.Photo);

				if (!commentUpdateVM.Photo.CheckFileSize(100))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
					return View(commentUpdateVM);
				}

				if (!commentUpdateVM.Photo.CheckFileFormat("image/"))
				{
					ModelState.AddModelError("Photo", "You must be choose image type");
					return View(commentUpdateVM);
				}

				var filename = String.Empty;

				try
				{
					filename = await commentUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "comments");
				}
				catch (Exception)
				{

					return View(commentUpdateVM);
				}
				comment.Photo = filename;
			}

			_repository.Update(comment);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var comment = await _repository.GetAsync(id);
			if (comment == null) return NotFound();
			return View(comment);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var comment = await _repository.GetAsync(id);
			if (comment == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "comments", comment.Photo);

			_repository.Delete(comment);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
