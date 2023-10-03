using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
	public AuthorRepository(AppDbContext context) : base(context)
	{
	}

	public async Task<Author> FirstInclude(int id)
	{
		return await _table.Include(a => a.Blogs).FirstOrDefaultAsync(a => a.Id == id);
	}

	public async Task<List<Author>> ListAsync()
	{
		return await _table.ToListAsync();
	}
}
