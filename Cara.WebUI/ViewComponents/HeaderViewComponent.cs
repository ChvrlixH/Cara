using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
	public readonly AppDbContext _context;

	public HeaderViewComponent(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		return View();
	}
}
