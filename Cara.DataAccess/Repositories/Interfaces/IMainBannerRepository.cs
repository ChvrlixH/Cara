using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IMainBannerRepository : IRepository<MainBannerItem>
{
    int Count();
}
