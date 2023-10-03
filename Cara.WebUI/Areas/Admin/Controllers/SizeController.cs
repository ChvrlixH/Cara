using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SizeController : Controller
    {
        private ISizeRepository _repository;

        public SizeController(ISizeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var sizes = await _repository.ListAsync();
            return View(sizes);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var size = await _repository.FirstThenInclude(id);

            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) { return NotFound(); }

            Size size = await _repository.GetAsync(id);
            if (size == null) { return NotFound(); }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Size editedSize)
        {
            if (id == null) { return NotFound(); }
            if (!ModelState.IsValid) return View(editedSize);
            Size size = await _repository.GetAsync(id);
            if (size == null) { return NotFound(); }

            bool isExist = _repository.AnyAsync(editedSize);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Name already exist");
                return View();
            }

            size.Name = editedSize.Name;

            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View(size);
            }

            bool isExist = _repository.AnyAsync(size);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Name already exist");
                return View();
            }

            Size newsize = new Size
            {
                Name = size.Name
            };

            _repository.CreateAsync(newsize);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            Size size = await _repository.GetAsync(id);
            if (size == null) { return NotFound(); }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            Size size = await _repository.GetAsync(id);
            if (size == null) { return NotFound(); }

            _repository.Delete(size);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
