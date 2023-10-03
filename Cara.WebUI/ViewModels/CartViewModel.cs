using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class CartViewModel
{
    public IEnumerable<CartBanner> CartBanners { get; set; } = null!;
}
