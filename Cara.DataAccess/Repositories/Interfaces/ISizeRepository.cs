using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface ISizeRepository : IRepository<Size> 
{
    public bool AnyAsync(Size editedSize);
    Task<List<Size>> ListAsync();
    Task<Size> FirstThenInclude(int id);
}
