using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.MainBanner;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class MainBannerController : Controller
    {
        private IMainBannerRepository _repository;
        private readonly IWebHostEnvironment _env;
        private int _count;

        public MainBannerController(IWebHostEnvironment env, IMainBannerRepository repository)
        {
            _env = env;
            _repository = repository;
            _count = _repository.Count();
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Count = _count;
            return View(await _repository.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {   
            var mainBanner = await _repository.GetAsync(id);
            if (mainBanner == null) return NotFound();
            return View(mainBanner);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MainBannerVM mainBannerVM)
        {
			if (!ModelState.IsValid) return View(mainBannerVM);
			if (mainBannerVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select Photo");
                return View(mainBannerVM);
            }

            if (!mainBannerVM.Photo.CheckFileSize(1000))
            {
				ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
				return View(mainBannerVM);
			}

            if (!mainBannerVM.Photo.CheckFileFormat("image/"))
            {
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(mainBannerVM);
			}

            //var wwwroot = _env.WebRootPath;
            //var filename = Guid.NewGuid().ToString() + mainBannerVM.Photo.FileName;
            //var resultPath = Path.Combine(wwwroot, "admin", "assets", "database", filename);

            //using (FileStream stream = new FileStream(resultPath, FileMode.Create))
            //{
            //    await mainBannerVM.Photo.CopyToAsync(stream);
            //}

            var filename = String.Empty;

            try
            {
                filename = await mainBannerVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "homebanners", "mainbanners");
            }
            catch (Exception)
            {

                return View(mainBannerVM);
            }

            MainBannerItem mainBanner = new()
            {
                Name = mainBannerVM.Name,
                Title= mainBannerVM.Title,
                Description = mainBannerVM.Description,
                BtnName= mainBannerVM.BtnName,
                Photo=filename
            };
            
           await _repository.CreateAsync(mainBanner);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));

		}



        public async Task<IActionResult> Update(int id)
        {
			var mainBanner = await _repository.GetAsync(id);
			if (mainBanner == null) return NotFound();

            MBannerUpdateVM bannerUpdateVM = new()
            {
                Name = mainBanner.Name,
                Title = mainBanner.Title,
                Description = mainBanner.Description,
                BtnName = mainBanner.BtnName,
                ImagePath = mainBanner.Photo
            };

            return View(bannerUpdateVM);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, MBannerUpdateVM bannerUpdateVM)
        {
            if (id != bannerUpdateVM.Id) return BadRequest();
			if (!ModelState.IsValid) return View(bannerUpdateVM);
			var mainBanner = await _repository.GetAsync(id);
			if (mainBanner == null) return NotFound();

            mainBanner.Name = bannerUpdateVM.Name;
            mainBanner.Title = bannerUpdateVM.Title;
            mainBanner.Description = bannerUpdateVM.Description;
            mainBanner.BtnName = bannerUpdateVM.BtnName;
            if (bannerUpdateVM.Photo != null)
            {
                Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "homebanners", "mainbanners", mainBanner.Photo);

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
					filename = await bannerUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "homebanners", "mainbanners");
				}
				catch (Exception)
				{

					return View(bannerUpdateVM);
				}
                mainBanner.Photo = filename;
			}

            _repository.Update(mainBanner);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
			if (_count == 1) return BadRequest();
			var mainBanner = await _repository.GetAsync(id);
            if (mainBanner == null) return NotFound();
            return View(mainBanner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
			if (_count == 1) return BadRequest();
			var mainBanner = await _repository.GetAsync(id);
            if (mainBanner == null) return NotFound();
            Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "homebanners", "mainbanners", mainBanner.Photo);

            _repository.Delete(mainBanner);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
