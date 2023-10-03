using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities.HeadBanners;

public class CartBanner : BaseEntity, IEntity
{
    public int Id { get; set; }
    public string? Photo { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = false;
}
