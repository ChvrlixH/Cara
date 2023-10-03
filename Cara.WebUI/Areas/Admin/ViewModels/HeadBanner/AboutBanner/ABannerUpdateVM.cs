using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.HeadBanner.AboutBanner;

public class ABannerUpdateVM
{
    public int Id { get; set; }
    public IFormFile? Photo { get; set; }
    [Required, MaxLength(80)]
    public string? Title { get; set; }
    [Required, MaxLength(180)]
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public string? ImagePath { get; set; }
}
