using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address> GetActiveAddressAsync();
}
