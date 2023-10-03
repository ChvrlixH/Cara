using Cara.Core.Entities.HeadBanners;

namespace Cara.DataAccess.Repositories.Interfaces.IHeadBanners;

public interface IAboutBannerRepository : IRepository<AboutBanner>
{
    Task<AboutBanner> GetActiveBannerAsync();
}
