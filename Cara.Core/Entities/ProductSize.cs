using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class ProductSize : IEntity
{
    public int Id { get; set; }
    public int SizeId { get; set; }
    public int ProductId { get; set; }
    public Size? Size { get; set; }
    public Product? Product { get; set; }
}
