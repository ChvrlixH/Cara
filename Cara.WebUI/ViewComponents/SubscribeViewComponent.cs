using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.ViewComponents;

public class SubscribeViewComponent : ViewComponent
{
	private readonly AppDbContext _context;

	public SubscribeViewComponent(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{

		return View();
	}
}
