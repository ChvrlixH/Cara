using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.SmallBanner;

public class SmallBannerVM
{
	[Required, MaxLength(50)]
	public string? Name { get; set; }
	[Required]
	public IFormFile? Photo { get; set; }
	public string? Title { get; set; }
}
