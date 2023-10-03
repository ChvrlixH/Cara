using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations.HeadBanners;

public class CartBannerRepository : Repository<CartBanner>, ICartBannerRepository
{
    public CartBannerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<CartBanner> GetActiveBannerAsync()
    {
        return await _table.FirstOrDefaultAsync(b => b.IsActive);
    }
}
