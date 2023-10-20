using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Repositories.Implementations
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Address> GetActiveAddressAsync()
        {
            return await _table.FirstOrDefaultAsync(a => a.IsActive);
        }
    }
}
