using Cara.Core.Entities.HeadBanners;

namespace Cara.DataAccess.Repositories.Interfaces.IHeadBanners;

public interface IContactBannerRepository : IRepository<ContactBanner>
{
    Task<ContactBanner> GetActiveBannerAsync();
}
