using Cara.Core.Entities;
using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class ContactViewModel
{
    public IEnumerable<ContactBanner> ContactBanners { get; set; } = null!;
    public IEnumerable<Address> Addresses { get; set; } = null!;
    public IEnumerable<Author> Authors { get; set; } = null!;
}
