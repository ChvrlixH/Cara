using Cara.Core.Entities.Common;
using Cara.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Cara.Core.Entities;

public class Address : BaseEntity, IEntity
{
    public int Id { get; set; }
    public string? HeadTitle { get; set; }
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? Map { get; set; }
    public string? Email { get; set; }  
    public string? Phone { get; set; }
    public string? Time { get; set; }
    public bool IsActive { get; set; } 
}
