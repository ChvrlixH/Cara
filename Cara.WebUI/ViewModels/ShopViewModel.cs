using Cara.Core.Entities;
using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class ShopViewModel
{
    public IEnumerable<ShopBanner> ShopBanners { get; set; } = null!;
    public IEnumerable<PCategory> PCategories { get; set; } = null!;
    public IEnumerable<Product> Products { get; set; } = null!;
    public IEnumerable<ProductImage> ProductImages { get; set; } = null!;
    public IEnumerable<Size> Sizes { get; set; } = null!;
}
