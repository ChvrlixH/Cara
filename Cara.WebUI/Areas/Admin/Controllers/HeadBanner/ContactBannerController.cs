using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Cara.WebUI.Areas.Admin.ViewModels.HeadBanner.ContactBanner;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers.HeadBanner
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class ContactBannerController : Controller
	{
		private IContactBannerRepository _repository;
		private readonly IWebHostEnvironment _env;
		public ContactBannerController(IContactBannerRepository repository, IWebHostEnvironment env)
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
		public async Task<IActionResult> Create(ContactBannerVM contactBannerVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (contactBannerVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(contactBannerVM);
			}

			if (!contactBannerVM.Photo.CheckFileSize(500))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
				return View(contactBannerVM);
			}

			if (!contactBannerVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(contactBannerVM);
			}

			if (contactBannerVM.IsActive)
			{
				var activebanner = await _repository.GetActiveBannerAsync();
				if (activebanner != null)
				{
					activebanner.IsActive = false;
				}
			}

			var filename = String.Empty;

			try
			{
				filename = await contactBannerVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "headbanners");
			}
			catch (Exception)
			{

				return View(contactBannerVM);
			}

			ContactBanner contactBanner = new()
			{
				Title = contactBannerVM.Title,
				Description = contactBannerVM.Description,
				Photo = filename,
				IsActive = contactBannerVM.IsActive
			};


			await _repository.CreateAsync(contactBanner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Detail(int id)
		{
			var banner = await _repository.GetAsync(id);
			if (banner == null) return NotFound();
			return View(banner);
		}

		public async Task<IActionResult> Update(int id)
		{
			var banner = await _repository.GetAsync(id);
			if (banner == null) return NotFound();

			CBannerUpdateVM bannerUpdateVM = new()
			{
				Title = banner.Title,
				Description = banner.Description,
				ImagePath = banner.Photo
			};


			return View(bannerUpdateVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, CBannerUpdateVM bannerUpdateVM)
		{
			if (id != bannerUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(bannerUpdateVM);
			var banner = await _repository.GetAsync(id);
			if (banner == null) return NotFound();

			if (bannerUpdateVM.IsActive)
			{
				var activebanner = await _repository.GetActiveBannerAsync();
				if (activebanner != null)
				{
					activebanner.IsActive = false;
				}
			}

			banner.Title = bannerUpdateVM.Title;
			banner.Description = bannerUpdateVM.Description;
			banner.IsActive = bannerUpdateVM.IsActive;
			if (bannerUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "headbanners", banner.Photo);

				if (!bannerUpdateVM.Photo.CheckFileSize(500))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
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
					filename = await bannerUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "headbanners");
				}
				catch (Exception)
				{

					return View(bannerUpdateVM);
				}
				banner.Photo = filename;
			}



			_repository.Update(banner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Delete(int id)
		{
			var banner = await _repository.GetAsync(id);
			if (banner == null) return NotFound();
			return View(banner);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]

		public async Task<IActionResult> DeletePost(int id)
		{
			var banner = await _repository.GetAsync(id);
			if (banner == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "headbanners", banner.Photo);

			_repository.Delete(banner);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
