using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations.HeadBanners;

public class AboutBannerRepository : Repository<AboutBanner>, IAboutBannerRepository
{
    public AboutBannerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<AboutBanner> GetActiveBannerAsync()
    {
        return await _table.FirstOrDefaultAsync(b => b.IsActive);
    }
}
