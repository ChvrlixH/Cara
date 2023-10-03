using Cara.Core.Entities;

namespace Cara.WebUI.ViewModels;

public class HomeViewModel
{
	public IEnumerable<HeroItem> HeroItems { get; set; } = null!;
	public IEnumerable<Feature> Features { get; set; } = null!;
	public IEnumerable<MainBannerItem> MainBannerItems { get; set; } = null!;
	public IEnumerable<SmallBannerItem> SmallBannerItems { get; set; } = null!;
	public IEnumerable<Subscribe> Subscribes { get; set; } = null!;
}
