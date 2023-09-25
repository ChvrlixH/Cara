namespace Cara.WebUI.Areas.Admin.ViewModels.Feature
{
	public class FeatureUpdateVM
	{
		public int Id { get; set; }
		public IFormFile? Photo { get; set; }
		public string? Title { get; set; }
        public string? ImagePath { get; set; }
    }
}
