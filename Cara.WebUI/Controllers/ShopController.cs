using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
	public class ShopController : Controller
	{
		private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			ShopViewModel shopViewModel = new()
			{
				PCategories = _context.PCategories.AsNoTracking(),
				ShopBanners = _context.ShopBanners.AsNoTracking()
			};

			return View(shopViewModel);
		}
	}
}
