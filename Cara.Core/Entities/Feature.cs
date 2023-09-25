using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities;

public class Feature : IEntity
{
	public int Id { get; set; }
	[Required]
	public string? Photo { get; set; }
	public string? Title { get; set; }
}
