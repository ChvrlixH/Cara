using Cara.DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Cara.WebUI.ViewComponents;

public class FeatureViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public FeatureViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var feature = _context.Features.Take(6).ToList();

        return View(feature);
    }
}
