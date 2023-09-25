using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IHeroItemRepository : IRepository<HeroItem>
{
	Task<HeroItem> GetActiveHeroAsync();
}
