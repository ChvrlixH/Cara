using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
    public class TermsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
