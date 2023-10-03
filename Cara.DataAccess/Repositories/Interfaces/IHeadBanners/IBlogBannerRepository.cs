using Cara.Core.Entities.HeadBanners;

namespace Cara.DataAccess.Repositories.Interfaces.IHeadBanners;

public interface IBlogBannerRepository : IRepository<BlogBanner>
{
    Task<BlogBanner> GetActiveBannerAsync();
}
