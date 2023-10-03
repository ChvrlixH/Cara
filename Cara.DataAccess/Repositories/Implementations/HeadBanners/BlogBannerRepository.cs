using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations.HeadBanners;

public class BlogBannerRepository : Repository<BlogBanner>, IBlogBannerRepository
{
    public BlogBannerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<BlogBanner> GetActiveBannerAsync()
    {
        return await _table.FirstOrDefaultAsync(b => b.IsActive);
    }
}
