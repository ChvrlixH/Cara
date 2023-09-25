using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
	public class SubscribeController : Controller
	{
		private readonly AppDbContext _context;

		public SubscribeController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> Create(string Email)
		{
			if (!_context.Subscribes.Any(s => s.Email == Email))
			{
				Subscribe subscribe = new() { Email = Email };
				await _context.Subscribes.AddAsync(subscribe);
				await _context.SaveChangesAsync();
			}
				return RedirectToAction("Index", "Home");
		}
	}
}
