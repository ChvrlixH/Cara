using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class BCategory : BaseEntity, IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public bool IsDeleted { get; set; }
	public ICollection<Blog>? Blogs { get; set; }
}
