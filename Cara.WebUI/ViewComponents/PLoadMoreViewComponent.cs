using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.ViewComponents;

public class PLoadMoreViewComponent :ViewComponent
{
	private readonly AppDbContext _context;

	public PLoadMoreViewComponent(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var products = _context.Products.Take(8).ToList();

		return View(products);
	}
}
