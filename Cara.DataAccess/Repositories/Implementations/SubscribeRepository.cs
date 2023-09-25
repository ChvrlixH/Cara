using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations;

public class SubscribeRepository : Repository<Subscribe>, ISubscribeRepository
{
	public SubscribeRepository(AppDbContext context) : base(context)
	{
	}

	public async Task<Subscribe> FirstAsync(int? id)
	{
		return await _table.FirstOrDefaultAsync(s => s.Id == id);
	}

	public async Task<List<Subscribe>> ListAsync()
	{
		return await _table.ToListAsync();
	}
}
