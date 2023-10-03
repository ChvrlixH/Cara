using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class AboutViewModel
{
    public IEnumerable<AboutBanner> AboutBanners { get; set; } = null!;
}
