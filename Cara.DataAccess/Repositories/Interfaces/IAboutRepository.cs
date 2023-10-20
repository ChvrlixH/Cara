using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IAboutRepository : IRepository<About>
{
	Task<About> GetActiveAboutAsync();
}
