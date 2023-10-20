using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations;

public class FaqRepository : Repository<Faq>, IFaqRepository
{
    public FaqRepository(AppDbContext context) : base(context)
    {
    }

}
