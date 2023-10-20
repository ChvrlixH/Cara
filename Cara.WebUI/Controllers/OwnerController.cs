using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
	public class OwnerController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
