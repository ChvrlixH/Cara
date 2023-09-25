using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Interfaces;

namespace Cara.DataAccess.Repositories.Implementations;

public class FeatureRepository : Repository<Feature>, IFeatureRepository
{
	public FeatureRepository(AppDbContext context) : base(context)
	{
	}
}
