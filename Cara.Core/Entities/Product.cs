using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Product : IEntity
{
    public int Id { get; set; }
    public string? Photo { get; set; }
    //public List<string>? SmallPhotos { get; set; }
    //public List<string>? Sizes { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? Desc { get; set; }
    public int Rating { get; set; }
    public string? Owner { get; set; }
    public bool IsDeleted { get; set; }
    public int PCategoryId { get; set; }
    public PCategory? pCategory { get; set; }
}
