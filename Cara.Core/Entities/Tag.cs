using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Tag : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public ICollection<BlogTags>? BlogTags { get; set; }
}
