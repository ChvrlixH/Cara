using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.SmallBanner;

public class SBannerUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
	public string? Name { get; set; }
	public IFormFile? Photo { get; set; }
	public string? Title { get; set; }
	public string? ImagePath { get; set; }
}
