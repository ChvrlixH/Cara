using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities
{
	public class HeroItem : IEntity
	{
		public int Id { get; set; }
		[Required]
		public string? Photo { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? Title { get; set; }
		public string? SubTitle { get; set; }
		public string? Description { get; set; }
		public bool isActive { get; set; } = false;
	}
}
