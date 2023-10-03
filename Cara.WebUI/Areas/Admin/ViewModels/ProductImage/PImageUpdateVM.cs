using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.ProductImage;

public class PImageUpdateVM
{
	public int Id { get; set; }
	public IFormFile? Photo { get; set; }
	[Required]
	public int ProductId { get; set; }
	public string? ImagePath { get; set; }
}
