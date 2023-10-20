using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.AuthorBlog;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class AuthorController : Controller
	{
		private readonly IAuthorRepository _repository;
		private readonly IWebHostEnvironment _env;

		public AuthorController(IAuthorRepository repository, IWebHostEnvironment env)
		{
			_repository = repository;
			_env = env;
		}


		public async Task<IActionResult> Index()
		{
			var author = await _repository.ListAsync();
			return View(author);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AuthorVM authorVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (authorVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(authorVM);
			}

			if (!authorVM.Photo.CheckFileSize(500))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
				return View(authorVM);
			}

			if (!authorVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(authorVM);
			}

			var filename = String.Empty;

			try
			{
				filename = await authorVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "authors");
			}
			catch (Exception)
			{

				return View(authorVM);
			}

			Author author = new()
			{
				Fullname = authorVM.Fullname,
				Profession = authorVM.Profession,
				Phone= authorVM.Phone,
				Email= authorVM.Email,
				Photo = filename
			};


			await _repository.CreateAsync(author);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Detail(int id)
		{
			var author = await _repository.FirstInclude(id);
			if (author == null) return NotFound();
			return View(author);
		}

		public async Task<IActionResult> Update(int id)
		{
			var author = await _repository.GetAsync(id);
			if (author == null) return NotFound();

			AuthorUpdateVM authorUpdateVM = new()
			{
				Fullname = author.Fullname,
				Profession = author.Profession,
				Phone = author.Phone,
				Email = author.Email,
				ImagePath = author.Photo
			};


			return View(authorUpdateVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, AuthorUpdateVM authorUpdateVM)
		{
			if (id != authorUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(authorUpdateVM);
			var author = await _repository.GetAsync(id);
			if (author == null) return NotFound();

			author.Fullname = authorUpdateVM.Fullname;
			author.Profession = authorUpdateVM.Profession;
			author.Phone = authorUpdateVM.Phone;
			author.Email = authorUpdateVM.Email;
			if (authorUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "authors", author.Photo);

				if (!authorUpdateVM.Photo.CheckFileSize(500))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
					return View(authorUpdateVM);
				}

				if (!authorUpdateVM.Photo.CheckFileFormat("image/"))
				{
					ModelState.AddModelError("Photo", "You must be choose image type");
					return View(authorUpdateVM);
				}
				var filename = String.Empty;

				try
				{
					filename = await authorUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "authors");
				}
				catch (Exception)
				{

					return View(authorUpdateVM);
				}
				author.Photo = filename;
			}



			_repository.Update(author);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Delete(int id)
		{
			var author = await _repository.GetAsync(id);
			if (author == null) return NotFound();
			return View(author);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]

		public async Task<IActionResult> DeletePost(int id)
		{
			var author = await _repository.GetAsync(id);
			if (author == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "authors", author.Photo);

			_repository.Delete(author);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
