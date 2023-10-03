using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.DataAccess.Repositories.Implementations;

public class BCategoryRepository : Repository<BCategory>, IBCategoryRepository
{
	public BCategoryRepository(AppDbContext context) : base(context)
	{
	}

	public bool AnyAsync(BCategory editedCategory)
	{
		string cleanedName = Regex.Replace(editedCategory.Name, @"\s+", " ").Trim();
		return _table.Any(t => t.Name == cleanedName);
	}

	public async Task<BCategory> FirstInclude(int id)
	{
		return await _table.Include(c => c.Blogs).FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
	}

	public async Task<List<BCategory>> ListAsync()
	{
		return await _table.ToListAsync();
	}
}
