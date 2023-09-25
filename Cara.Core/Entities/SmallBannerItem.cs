using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities;

public class SmallBannerItem : IEntity
{
	public int Id { get; set; }
	[Required]
	public string? Name { get; set; }
	[Required]
	public string? Photo { get; set; }
	public string? Title { get; set; }
}
