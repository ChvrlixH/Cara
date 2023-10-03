namespace Cara.Core.Entities.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    //public string? CreatedBy { get; set; }
    //public string? ModifiedBy { get; set; }
}
