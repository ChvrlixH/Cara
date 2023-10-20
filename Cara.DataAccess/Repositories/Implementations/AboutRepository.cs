using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations;

public class AboutRepository : Repository<About>, IAboutRepository
{
	public AboutRepository(AppDbContext context) : base(context)
	{
	}

	public async Task<About> GetActiveAboutAsync()
	{
		return await _table.FirstOrDefaultAsync(a => a.IsActive);
	}
}
