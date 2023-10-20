using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ContactViewModel contactViewModel = new()
            {
                ContactBanners = _context.ContactBanners.AsNoTracking(),
                Addresses = _context.Addresses.AsNoTracking(),
                Authors = _context.Authors.AsNoTracking()
            };
            return View(contactViewModel);
        }
    }
}
