using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Areas.Admin.ViewModels.Team;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class TeamController : Controller
    {
        private readonly ITeamRepository _repository;
        private readonly IWebHostEnvironment _env;
        public TeamController(ITeamRepository repository, IWebHostEnvironment env)
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
            var teamItem = await _repository.GetAsync(id);
            if (teamItem == null) { return NotFound(); }
            return View(teamItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamVM teamVM)
        {
            if (!ModelState.IsValid) return View(teamVM);
            if (teamVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select Photo");
                return View(teamVM);
            }

            if (!teamVM.Photo.CheckFileSize(100))
            {
                ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
                return View(teamVM);
            }

            if (!teamVM.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "You must be choose image type");
                return View(teamVM);
            }

            var filename = String.Empty;

            try
            {
                filename = await teamVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "teams");
            }
            catch (Exception)
            {

                return View(teamVM);
            }

            Team teamItem = new()
            {
                Fullname = teamVM.Fullname,
                Photo = filename
            };

            await _repository.CreateAsync(teamItem);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var team = await _repository.GetAsync(id);
            if (team == null) return NotFound();

            TeamUpdateVM teamUpdateVM = new()
            {
                Fullname = team.Fullname,
                ImagePath = team.Photo
            };

            return View(teamUpdateVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TeamUpdateVM teamUpdateVM)
        {
            if (id != teamUpdateVM.Id) return BadRequest();
            if (!ModelState.IsValid) return View(teamUpdateVM);
            var team = await _repository.GetAsync(id);
            if (team == null) return NotFound();

            team.Fullname = teamUpdateVM.Fullname;
            if (teamUpdateVM.Photo != null)
            {
                Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "teams", team.Photo);

                if (!teamUpdateVM.Photo.CheckFileSize(100))
                {
                    ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
                    return View(teamUpdateVM);
                }

                if (!teamUpdateVM.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "You must be choose image type");
                    return View(teamUpdateVM);
                }

                var filename = String.Empty;

                try
                {
                    filename = await teamUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "teams");
                }
                catch (Exception)
                {

                    return View(teamUpdateVM);
                }
                team.Photo = filename;
            }

            _repository.Update(team);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _repository.GetAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var team = await _repository.GetAsync(id);
            if (team == null) return NotFound();
            Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "teams", team.Photo);

            _repository.Delete(team);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
