using Cara.Core.Entities;
using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class ShopViewModel
{
    public IEnumerable<PCategory> PCategories { get; set; } = null!;
    public IEnumerable<ShopBanner> ShopBanners { get; set; } = null!;
}
