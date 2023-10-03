using Cara.Core.Entities.HeadBanners;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations.HeadBanners;

public class ContactBannerRepository : Repository<ContactBanner>, IContactBannerRepository
{
    public ContactBannerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ContactBanner> GetActiveBannerAsync()
    {
        return await _table.FirstOrDefaultAsync(b => b.IsActive);
    }
}
