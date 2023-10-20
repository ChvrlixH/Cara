using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.About;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class AboutController : Controller
	{
		public readonly IAboutRepository _repository;
		public readonly IWebHostEnvironment _env;

		public AboutController(IAboutRepository repository, IWebHostEnvironment env)
		{
			_repository = repository;
			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _repository.GetAllAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AboutVM aboutVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (aboutVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(aboutVM);
			}

			if (!aboutVM.Photo.CheckFileSize(1000))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
				return View(aboutVM);
			}

			if (!aboutVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(aboutVM);
			}

			if (aboutVM.IsActive)
			{
				var activeabout = await _repository.GetActiveAboutAsync();
				if (activeabout != null)
				{
					activeabout.IsActive = false;
				}
			}

			var filename = String.Empty;

			try
			{
				filename = await aboutVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "abouts");
			}
			catch (Exception)
			{

				return View(aboutVM);
			}

			About about = new()
			{
				Title = aboutVM.Title,
				Description = aboutVM.Description,
				DescDotted = aboutVM.DescDotted,
				DescMarquee = aboutVM.DescMarquee,
				Photo = filename,
				IsActive = aboutVM.IsActive
			};


			await _repository.CreateAsync(about);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Detail(int id)
		{
			var about = await _repository.GetAsync(id);
			if (about == null) return NotFound();
			return View(about);
		}

		public async Task<IActionResult> Update(int id)
		{
			var about = await _repository.GetAsync(id);
			if (about == null) return NotFound();

			AboutUpdateVM aboutUpdateVM = new()
			{
				Title = about.Title,
				Description = about.Description,
				DescDotted = about.DescDotted,
				DescMarquee = about.DescMarquee,
				ImagePath = about.Photo
			};


			return View(aboutUpdateVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, AboutUpdateVM aboutUpdateVM)
		{
			if (id != aboutUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(aboutUpdateVM);
			var about = await _repository.GetAsync(id);
			if (about == null) return NotFound();

			if (aboutUpdateVM.IsActive)
			{
				var activeabout = await _repository.GetActiveAboutAsync();
				if (activeabout != null)
				{
					activeabout.IsActive = false;
				}
			}

			about.Title = aboutUpdateVM.Title;
			about.Description = aboutUpdateVM.Description;
			about.DescDotted = aboutUpdateVM.DescDotted;
			about.DescMarquee = aboutUpdateVM.DescMarquee;
			about.IsActive = aboutUpdateVM.IsActive;
			if (aboutUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "abouts", about.Photo);

				if (!aboutUpdateVM.Photo.CheckFileSize(1000))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
					return View(aboutUpdateVM);
				}

				if (!aboutUpdateVM.Photo.CheckFileFormat("image/"))
				{
					ModelState.AddModelError("Photo", "You must be choose image type");
					return View(aboutUpdateVM);
				}
				var filename = String.Empty;

				try
				{
					filename = await aboutUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "abouts");
				}
				catch (Exception)
				{

					return View(aboutUpdateVM);
				}
				about.Photo = filename;
			}



			_repository.Update(about);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Delete(int id)
		{
			var about = await _repository.GetAsync(id);
			if (about == null) return NotFound();
			return View(about);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]

		public async Task<IActionResult> DeletePost(int id)
		{
			var about = await _repository.GetAsync(id);
			if (about == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "abouts", about.Photo);

			_repository.Delete(about);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
