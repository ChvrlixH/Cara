using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;

namespace Cara.Core.Entities;

public class Team : BaseEntity, IEntity
{
    public int Id { get; set; }
    public string? Fullname { get; set; }
    public string? Photo { get; set;}
}
