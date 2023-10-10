using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            // 404 hatası için özel sayfayı döndürün
            if (statusCode == 404)
            {
                return View("404");
            }

            // Diğer hatalar için genel bir hata sayfasını döndürün
            return View("Error");
        }
    }
}
