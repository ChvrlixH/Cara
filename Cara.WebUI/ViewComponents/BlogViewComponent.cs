using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.ViewComponents;

public class BlogViewComponent : ViewComponent
{
	private readonly AppDbContext _context;

	public BlogViewComponent(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var blogs = _context.Blogs.Include(b => b.BCategory).OrderByDescending(b=>b.ModifiedAt).Take(3).ToList();

		return View(blogs);
	}
}
