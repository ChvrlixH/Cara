using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.SmallBanner;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize( Roles ="Admin,Moderator")]
	public class SmallBannerController : Controller
	{
		public ISmallBannerRepository _repository;
		public IWebHostEnvironment _env;
		public SmallBannerController(ISmallBannerRepository repository, IWebHostEnvironment env)
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
			var smallBanner = await _repository.GetAsync(id);
			if(smallBanner == null) return NotFound();
			return View(smallBanner);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SmallBannerVM smallBannerVM)
		{
			if (!ModelState.IsValid) return View(smallBannerVM);
			if (smallBannerVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(smallBannerVM);
			}

			if (!smallBannerVM.Photo.CheckFileSize(1000))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
				return View(smallBannerVM);
			}

			if (!smallBannerVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(smallBannerVM);
			}

			var filename = String.Empty;

			try
			{
				filename = await smallBannerVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database");
			}
			catch (Exception)
			{

				return View(smallBannerVM);
			}

			 SmallBannerItem smallBanner = new()
			{
				Name = smallBannerVM.Name,
				Title = smallBannerVM.Title,
				Photo = filename
			};

			await _repository.CreateAsync(smallBanner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));

		}



		public async Task<IActionResult> Update(int id)
		{
			var smallBanner = await _repository.GetAsync(id);
			if (smallBanner == null) return NotFound();

			SBannerUpdateVM bannerUpdateVM = new()
			{
				Name = smallBanner.Name,
				Title = smallBanner.Title,
				ImagePath = smallBanner.Photo
			};

			return View(bannerUpdateVM);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, SBannerUpdateVM bannerUpdateVM)
		{
			if (id != bannerUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(bannerUpdateVM);
			var smallBanner = await _repository.GetAsync(id);
			if (smallBanner == null) return NotFound();

			smallBanner.Name = bannerUpdateVM.Name;
			smallBanner.Title = bannerUpdateVM.Title;
			if (bannerUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", smallBanner.Photo);

				if (!bannerUpdateVM.Photo.CheckFileSize(1000))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
					return View(bannerUpdateVM);
				}

				if (!bannerUpdateVM.Photo.CheckFileFormat("image/"))
				{
					ModelState.AddModelError("Photo", "You must be choose image type");
					return View(bannerUpdateVM);
				}

				var filename = String.Empty;

				try
				{
					filename = await bannerUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database");
				}
				catch (Exception)
				{

					return View(bannerUpdateVM);
				}
				smallBanner.Photo = filename;
			}

			_repository.Update(smallBanner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var smallBanner = await _repository.GetAsync(id);
			if (smallBanner == null) return NotFound();
			return View(smallBanner);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var smallBanner = await _repository.GetAsync(id);
			if (smallBanner == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", smallBanner.Photo);

			_repository.Delete(smallBanner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
