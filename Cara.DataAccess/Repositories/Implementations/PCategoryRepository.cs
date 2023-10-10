using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.DataAccess.Repositories.Implementations;

public class PCategoryRepository : Repository<PCategory>, IPCategoryRepository
{
    private readonly AppDbContext _context;
    public PCategoryRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public bool AnyAsync(PCategory editedCategory)
	{
		string cleanedName = Regex.Replace(editedCategory.Name, @"\s+", " ").Trim();
		return _table.Any(t => t.Name == cleanedName);
	}

    public async Task DeleteCategoryAndRelatedProductsAsync(PCategory category)
    {
        var relatedProducts = _context.Products.Where(b => b.PCategoryId == category.Id);
        _context.Products.RemoveRange(relatedProducts);

        category.IsDeleted = true;
        _context.PCategories.Update(category);

        await _context.SaveChangesAsync();
    }

    public async Task<PCategory> FirstInclude(int id)
	{
		return await _table.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
	}

	public async Task<List<PCategory>> ListAsync()
	{
		return await _table.ToListAsync();
	}
}
