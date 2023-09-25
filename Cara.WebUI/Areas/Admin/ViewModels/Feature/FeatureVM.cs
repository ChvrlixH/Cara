using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Feature
{
	public class FeatureVM
	{
		[Required]
		public IFormFile? Photo { get; set; }
		public string? Title { get; set; }
	}
}
