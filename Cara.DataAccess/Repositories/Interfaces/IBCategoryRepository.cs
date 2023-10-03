using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IBCategoryRepository : IRepository<BCategory> 
{
	public bool AnyAsync(BCategory editedCategory);
	Task<List<BCategory>> ListAsync();
	Task<BCategory> FirstInclude(int id);
}
