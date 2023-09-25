using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            HomeViewModel homeViewModel = new()
            {
                HeroItems = _context.HeroItems.AsNoTracking(),
                Features = _context.Features.AsNoTracking(),
                MainBannerItems= _context.MainBannerItems.AsNoTracking(),
                SmallBannerItems = _context.SmallBannerItems.AsNoTracking(),
                Subscribes = _context.Subscribes.AsNoTracking()
            };
            return View(homeViewModel);
        }
    }
}
