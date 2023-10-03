using Cara.Core.Entities;

namespace Cara.DataAccess.Repositories.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
	public bool AnyAsync(Tag editedtag);
	Task<List<Tag>> ListAsync();
	Task<Tag> FirstThenInclude(int id);
}
