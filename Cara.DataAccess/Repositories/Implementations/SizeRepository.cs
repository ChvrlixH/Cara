using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cara.DataAccess.Repositories.Implementations;

public class SizeRepository : Repository<Size>, ISizeRepository
{
    public SizeRepository(AppDbContext context) : base(context)
    {
    }

    public bool AnyAsync(Size editedSize)
    {
        string cleanedName = Regex.Replace(editedSize.Name, @"\s+", " ").Trim();
        return _table.Any(t => t.Name == cleanedName);
    }

    public async Task<Size> FirstThenInclude(int id)
    {
        return await _table.Include(c => c.ProductSizes).ThenInclude(bt => bt.Product).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Size>> ListAsync()
    {
        return await _table.ToListAsync();
    }
}
