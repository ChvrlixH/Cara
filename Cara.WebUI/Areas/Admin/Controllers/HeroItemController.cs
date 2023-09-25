using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.HeroItem;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class HeroItemController : Controller
    {
		private IHeroItemRepository _repository;
		private readonly IWebHostEnvironment _env;
		public HeroItemController(IWebHostEnvironment env, IHeroItemRepository repository)
		{
			_env = env;
            _repository = repository;
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
        public async Task<IActionResult> Create(HeroItemVM heroItemVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
			if (heroItemVM.Photo == null)
			{
				ModelState.AddModelError("Photo", "Select Photo");
                return View(heroItemVM);
			}

			if (!heroItemVM.Photo.CheckFileSize(1500))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 1.5 mb");
				return View(heroItemVM);
			}

			if (!heroItemVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(heroItemVM);
			}

			if (heroItemVM.isActive)
			{
				var activeHero = await _repository.GetActiveHeroAsync();
				if (activeHero != null)
				{
					activeHero.isActive = false;
				}
			}

			var filename = String.Empty;

			try
			{
				filename = await heroItemVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database");
			}
			catch (Exception)
			{

				return View(heroItemVM);
            }

            HeroItem heroItem = new()
            {
                Name = heroItemVM.Name,
                Title= heroItemVM.Title,
                SubTitle= heroItemVM.SubTitle,
                Description= heroItemVM.Description,
                Photo= filename,
                isActive=heroItemVM.isActive
            };


            await _repository.CreateAsync(heroItem);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
		{
			var hero = await _repository.GetAsync(id);
            if (hero == null) return NotFound();
			return View(hero);
		}

		public async Task<IActionResult> Update(int id)
		{
			var hero = await _repository.GetAsync(id);
			if (hero == null) return NotFound();

            HItemUpdateVM itemUpdateVM = new()
            {
                Name = hero.Name,
                Title = hero.Title,
                SubTitle = hero.SubTitle,
                Description = hero.Description,
                ImagePath = hero.Photo
            };


            return View(itemUpdateVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, HItemUpdateVM itemUpdateVM)
		{
            if(id!= itemUpdateVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(itemUpdateVM);
            var hero = await _repository.GetAsync(id);
            if (hero == null) return NotFound();

			if (itemUpdateVM.isActive)
			{
				var activeHero = await _repository.GetActiveHeroAsync();
				if (activeHero != null)
				{
					activeHero.isActive = false;
				}
			}

			hero.Name = itemUpdateVM.Name;
            hero.Title = itemUpdateVM.Title;
            hero.SubTitle = itemUpdateVM.SubTitle;
            hero.Description = itemUpdateVM.Description;
            hero.isActive = itemUpdateVM.isActive;
			if (itemUpdateVM.Photo != null)
			{
				Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", hero.Photo);

				if (!itemUpdateVM.Photo.CheckFileSize(1500))
				{
					ModelState.AddModelError("Photo", "Image size must be less than 1.5 mb");
					return View(itemUpdateVM);
				}

				if (!itemUpdateVM.Photo.CheckFileFormat("image/"))
				{
					ModelState.AddModelError("Photo", "You must be choose image type");
					return View(itemUpdateVM);
				}
				var filename = String.Empty;

				try
				{
					filename = await itemUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database");
				}
				catch (Exception)
				{

					return View(itemUpdateVM);
				}
				hero.Photo = filename;
			}



			_repository.Update(hero);
			await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Delete(int id)
		{
            var hero = await _repository.GetAsync(id);
            if (hero == null) return NotFound();
			return View(hero);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeletePost(int id)
        {
            var hero = await _repository.GetAsync(id);
            if (hero == null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", hero.Photo);

			_repository.Delete(hero);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
