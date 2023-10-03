using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class ProductImage : BaseEntity,IEntity
{
    public int Id { get; set; }
    public string? Photo { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
