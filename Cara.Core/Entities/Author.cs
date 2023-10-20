using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Author : IEntity
{
	public int Id { get; set; }
	public string? Fullname { get; set; }
	public string? Photo { get; set; }
	public string? Profession { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public ICollection<Blog>? Blogs { get; set; }
}
