using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IPCategoryRepository : IRepository<PCategory>
{
	public bool AnyAsync(PCategory editedCategory);
	Task<List<PCategory>> ListAsync();
	Task<PCategory> FirstInclude(int id);
}
