using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
