using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]
    public class FaqController : Controller
    {
        private readonly IFaqRepository _repository;

        public FaqController(IFaqRepository repository)
        {
            _repository = repository;
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
		public async Task<IActionResult> Create(Faq faq)
		{
			if (!ModelState.IsValid)
			{
				return View(faq);
			}

			Faq newfaq = new Faq
			{
				Question = faq.Question,
                Answer = faq.Answer
			};

			_repository.CreateAsync(newfaq);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) { return NotFound(); }

            Faq faq = await _repository.GetAsync(id);
            if (faq == null) { return NotFound(); }
            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Faq editedfaq)
        {
            if (id == null) { return NotFound(); }
            if (!ModelState.IsValid) return View(editedfaq);
            Faq faq = await _repository.GetAsync(id);
            if (faq == null) { return NotFound(); }


            faq.Question = editedfaq.Question;
            faq.Answer = editedfaq.Answer;

            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            Faq faq = await _repository.GetAsync(id);
            if (faq == null) { return NotFound(); }
            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            Faq faq = await _repository.GetAsync(id);
            if (faq == null) { return NotFound(); }

            _repository.Delete(faq);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
