using Cara.Business.Helpers;
using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Cara.Core.Entities;

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
                Authors = _context.Authors.AsNoTracking()
            };
            return View(contactViewModel);
        }
    }
}
