using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations;

public class HeroItemRepository : Repository<HeroItem>, IHeroItemRepository
{
	public HeroItemRepository(AppDbContext context) : base(context)
	{
	}
	public async Task<HeroItem> GetActiveHeroAsync()
	{
		return await _table.FirstOrDefaultAsync(h => h.isActive);
	}
}
