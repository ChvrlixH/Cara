using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Size : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<ProductSize>? ProductSizes { get; set; }
}
