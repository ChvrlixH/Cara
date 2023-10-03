using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
	public ProductImageRepository(AppDbContext context) : base(context)
	{
	}

	public bool DoesPImageExist(int productId)
	{
		return _table.Any(pi => pi.Id == productId);
	}

	public async Task<ProductImage> FirstInclude(int id)
	{
		return await _table.Include(pi => pi.Product).FirstOrDefaultAsync(pi => pi.Id == id);
	}

	public IEnumerable<ProductImage> GetAllProduct()
	{
		return _table.AsEnumerable();
	}

	public async Task<List<ProductImage>> ListIncludeAsync()
	{
		return await _table.Include(p=>p.Product).ToListAsync();
	}
}
