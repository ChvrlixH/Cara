using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
	public class ShopController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
