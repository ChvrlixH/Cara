using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
	public class TeamController : Controller
	{
		private readonly AppDbContext _context;

		public TeamController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			TeamViewModel teamViewModel = new()
			{
				Teams = _context.Teams.AsNoTracking(),
				Comments= _context.Comments.AsNoTracking()
			};

			return View(teamViewModel);
		}
	}
}
