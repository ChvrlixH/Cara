using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
    public class FaqController : Controller
    {
        private readonly AppDbContext _context;

		public FaqController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            FaqViewModel faqViewModel = new()
            {
                Faqs= _context.Faqs.AsNoTracking()
            };
            return View(faqViewModel);
        }
    }
}
