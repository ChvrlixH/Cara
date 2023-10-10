using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.ViewComponents;

public class ProductViewComponent : ViewComponent
{
	private readonly AppDbContext _context;

	public ProductViewComponent(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync(bool takeFour, bool orderByCreated)
	{
		IQueryable<Product> query = _context.Products;

		if (orderByCreated)
		{
			query = query.OrderByDescending(p => p.CreatedAt);
		}
		else
		{
			query = query.OrderByDescending(p => p.ModifiedAt);
		}

		if (takeFour)
		{
			var products = await query.Take(4).ToListAsync();
			return View(products);
		}
		else
		{
			var products = await query.Take(8).ToListAsync();
			return View(products);
		}
	}
}

