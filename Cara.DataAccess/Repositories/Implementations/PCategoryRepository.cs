using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.DataAccess.Repositories.Implementations;

public class PCategoryRepository : Repository<PCategory>, IPCategoryRepository
{
	public PCategoryRepository(AppDbContext context) : base(context)
	{
	}

	public bool AnyAsync(PCategory editedCategory)
	{
		string cleanedName = Regex.Replace(editedCategory.Name, @"\s+", " ").Trim();
		return _table.Any(t => t.Name == cleanedName);
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
