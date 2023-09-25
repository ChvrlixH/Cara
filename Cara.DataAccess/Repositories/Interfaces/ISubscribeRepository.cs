using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface ISubscribeRepository : IRepository<Subscribe>
{
	Task<List<Subscribe>> ListAsync();
	Task<Subscribe> FirstAsync(int? id);
}
