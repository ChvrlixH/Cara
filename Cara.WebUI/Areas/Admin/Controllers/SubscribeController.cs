using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Cara.WebUI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class SubscribeController : Controller
	{
		private ISubscribeRepository _repository;

		public SubscribeController(ISubscribeRepository repository)
		{
			_repository = repository;
		}

		public async Task<IActionResult> Index()
		{
			List<Subscribe> subscribes = await _repository.ListAsync();
			return View(subscribes);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			Subscribe? subscribe = await _repository.FirstAsync(id);
			if (subscribe is null) return NotFound();
			return View(subscribe);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		[ActionName("Delete")]
		public async Task<IActionResult> DeleteSubscribe(int id)
		{
			Subscribe? subscribe = await _repository.FirstAsync(id);
			if (subscribe is null) return NotFound();

			_repository.Delete(subscribe);
			await _repository.SaveAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
