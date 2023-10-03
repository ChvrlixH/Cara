using Cara.Core.Entities.HeadBanners;

namespace Cara.DataAccess.Repositories.Interfaces.IHeadBanners;

public interface ICartBannerRepository : IRepository<CartBanner>
{
    Task<CartBanner> GetActiveBannerAsync();
}
