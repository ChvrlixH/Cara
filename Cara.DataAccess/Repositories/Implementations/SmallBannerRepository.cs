using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations;

public class SmallBannerRepository : Repository<SmallBannerItem>, ISmallBannerRepository
{
	public SmallBannerRepository(AppDbContext context) : base(context)
	{
	}
}
