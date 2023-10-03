using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Product : BaseEntity,IEntity
{
    public int Id { get; set; }
    public string? Photo { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public int Rating { get; set; }
    public string? Owner { get; set; }
    public int PCategoryId { get; set; }
    public PCategory? PCategory { get; set; }
    public ICollection<ProductImage>? ProductImages { get; set; }
    public ICollection<ProductSize>? ProductSizes { get; set; } 
}
