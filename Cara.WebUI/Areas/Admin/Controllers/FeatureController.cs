using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.Feature;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles ="Admin,Moderator")]
	public class FeatureController : Controller
	{
		private IFeatureRepository _repository;
		private readonly IWebHostEnvironment _env;

		public FeatureController(IFeatureRepository repository, IWebHostEnvironment env)
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
			var featureItem = await _repository.GetAsync(id);
			if(featureItem == null) { return NotFound(); }
			return View(featureItem);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(FeatureVM featureVM)
		{
			if (!ModelState.IsValid) return View(featureVM);
			if (featureVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
				return View(featureVM);
			}

			if (!featureVM.Photo.CheckFileSize(100))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
				return View(featureVM);
			}

			if (!featureVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(featureVM);
			}

			var filename = String.Empty;

			try
			{
				filename = await featureVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "features");
			}
			catch (Exception)
			{

				return View(featureVM);
			}

			Feature featureItem = new()
			{
				Title = featureVM.Title,
				Photo = filename
			};

			await _repository.CreateAsync(featureItem);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Update(int id)
        {
            var feature = await _repository.GetAsync(id);
            if (feature == null) return NotFound();

            FeatureUpdateVM featureUpdateVM = new()
            {
                Title = feature.Title,
                ImagePath = feature.Photo
            };

            return View(featureUpdateVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, FeatureUpdateVM featureUpdateVM)
        {
            if (id != featureUpdateVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(featureUpdateVM);
            var feature = await _repository.GetAsync(id);
            if (feature == null) return NotFound();

            feature.Title = featureUpdateVM.Title;
            if (featureUpdateVM.Photo != null)
            {
                Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "features", feature.Photo);

                if (!featureUpdateVM.Photo.CheckFileSize(100))
                {
                    ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
                    return View(featureUpdateVM);
                }

                if (!featureUpdateVM.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "You must be choose image type");
                    return View(featureUpdateVM);
                }

                var filename = String.Empty;

                try
                {
                    filename = await featureUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "features");
                }
                catch (Exception)
                {

                    return View(featureUpdateVM);
                }
                feature.Photo = filename;
            }

            _repository.Update(feature);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _repository.GetAsync(id);
            if (feature == null) return NotFound();
            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var feature = await _repository.GetAsync(id);
            if (feature == null) return NotFound();
            Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "features", feature.Photo);

            _repository.Delete(feature);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
