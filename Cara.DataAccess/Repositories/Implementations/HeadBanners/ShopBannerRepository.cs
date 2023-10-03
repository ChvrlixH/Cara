using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations.HeadBanners;

public class ShopBannerRepository : Repository<ShopBanner>, IShopBannerRepository
{
    public ShopBannerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ShopBanner> GetActiveBannerAsync()
    {
        return await _table.FirstOrDefaultAsync(b => b.IsActive);
    }
}
