using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/ErrorPage/Error404")]
        public IActionResult Index()
        {
            return View("Error404");
        }
    }
}
