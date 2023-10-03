using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
	Task<List<Author>> ListAsync();
	Task<Author> FirstInclude(int id);
}
