using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.About;

public class AboutVM
{
	[Required]
	public IFormFile? Photo { get; set; }
	[Required, MaxLength(100)]
	public string? Title { get; set; }
	[Required, MaxLength(500)]
	public string? Description { get; set; }
	[MaxLength(250)]
	public string? DescDotted { get; set; }
	[MaxLength(250)]
	public string? DescMarquee { get; set; }
	public bool IsActive { get; set; } = false;
}
