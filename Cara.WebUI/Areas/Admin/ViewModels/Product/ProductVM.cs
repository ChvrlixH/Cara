using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Product;

public class ProductVM
{
	public int Id { get; set; }
	[Required]
	public IFormFile? Photo { get; set; }
	[Required, MaxLength(120)]
	public string? Name { get; set; }
	[Required,DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true),Range(10,9999.99)]
	public decimal Price { get; set; }
	[Required,MaxLength(600)]
	public string? Description { get; set; }
	[Required,Range(1,5)]
	public int Rating { get; set; }
	[Required, MaxLength(80)]
	public string? Owner { get; set; }
	public int PCategoryId { get; set; }
	[Required]
	public int[]? SizeIds { get; set; }
}
