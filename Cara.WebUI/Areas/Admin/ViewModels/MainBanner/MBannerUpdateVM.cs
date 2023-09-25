using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.MainBanner
{
	public class MBannerUpdateVM
	{
		public int? Id { get; set; }
		public IFormFile? Photo { get; set; }
		[Required, MaxLength(50)]
		public string? Name { get; set; }
		[Required, MaxLength(50)]
		public string? Title { get; set; }
		[MaxLength(100)]
		public string? Description { get; set; }
		[MaxLength(40)]
		public string? BtnName { get; set; }
		public string? ImagePath { get; set; }
	}
}
