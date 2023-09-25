using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations
{
    public class MainBannerRepository : Repository<MainBannerItem>, IMainBannerRepository
    {
        public MainBannerRepository(AppDbContext context) : base(context)
        {
        }

        public int Count()
        {
            return _table.Count();
        }
    }
}
