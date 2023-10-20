using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class About : BaseEntity, IEntity
{
	public int Id { get; set; }
	public string? Photo { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public string? DescDotted { get; set; }
	public string? DescMarquee { get; set; }
	public bool IsActive { get; set; }

}
