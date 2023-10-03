using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Blog : BaseEntity, IEntity
{
	public int Id { get; set; }
	public string? Title { get; set; }
	public string? Photo { get; set; }
	public string? Description { get; set; }
	public int BCategoryId { get; set; }
	public int AuthorId { get; set; }
	public BCategory? BCategory { get; set; }
	public Author? Author { get; set; }
	public ICollection<BlogTags>? BlogTags { get; set; }
}
