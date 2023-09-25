using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities
{
	public class MainBannerItem : IEntity
	{
		public int Id { get; set; }
		[Required]
		public string? Photo { get; set; }
		[Required, MaxLength(40)]
		public string? Name { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? BtnName { get; set; }
	}
}
