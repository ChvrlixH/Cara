using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.DataAccess.Repositories.Implementations;

public class TagRepository : Repository<Tag>, ITagRepository
{
	public TagRepository(AppDbContext context) : base(context)
	{
	}

	public bool AnyAsync(Tag editedtag)
	{
		string cleanedName = Regex.Replace(editedtag.Name, @"\s+", " ").Trim();
		return _table.Any(t => t.Name == cleanedName);
	}

	public async Task<Tag> FirstThenInclude(int id)
	{
		return await _table.Include(c => c.BlogTags).ThenInclude(bt => bt.Blog).FirstOrDefaultAsync(c => c.Id == id);
	}

	public async Task<List<Tag>> ListAsync()
	{
		return await _table.ToListAsync();
	}
}
