using Cara.Core.Entities;
using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class AboutViewModel
{
    public IEnumerable<AboutBanner> AboutBanners { get; set; } = null!;
    public IEnumerable<About> Abouts { get; set; } = null!;
}
