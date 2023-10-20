using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations;

public class TeamRepository : Repository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context)
    {
    }
}
