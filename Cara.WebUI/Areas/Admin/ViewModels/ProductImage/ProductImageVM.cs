using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.ProductImage;

public class ProductImageVM
{
	[Required]
	public IFormFile? Photo { get; set; }
	[Required]
	public int ProductId { get; set; }
}
