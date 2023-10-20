using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities;

public class Faq : BaseEntity, IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string? Question { get; set; }
    [Required, MaxLength(400)]
    public string? Answer { get; set; }
}
