using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Product;

public class ProductUpdateVM
{
	public int Id { get; set; }
	public IFormFile? Photo { get; set; }
	[Required, MaxLength(50)]
	public string? Name { get; set; }
	[Required, DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true), Range(10, 9999.99)]
	public decimal Price { get; set; }
	[Required, MaxLength(400)]
	public string? Description { get; set; }
	[Required, Range(1, 5)]
	public int Rating { get; set; }
	[Required, MaxLength(60)]
	public string? Owner { get; set; }
	public int PCategoryId { get; set; }
	public int[]? SizeIds { get; set; }
	public string? ImagePath { get; set; }
}
