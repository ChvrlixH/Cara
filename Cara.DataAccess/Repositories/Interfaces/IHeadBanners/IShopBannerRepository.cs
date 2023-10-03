using Cara.Core.Entities.HeadBanners;

namespace Cara.DataAccess.Repositories.Interfaces.IHeadBanners;

public interface IShopBannerRepository : IRepository<ShopBanner>
{
    Task<ShopBanner> GetActiveBannerAsync();
}
