using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Address;

public class AddressUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string? HeadTitle { get; set; }
    [Required, MaxLength(150)]
    public string? Title { get; set; }
    [Required, MaxLength(100)]
    public string? SubTitle { get; set; }
    [Required, MaxLength(150)]
    public string? Map { get; set; }
    [Required, MaxLength(256), DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required, MaxLength(50), DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }
    [Required, MaxLength(150)]
    public string? Time { get; set; }
    public bool IsActive { get; set; }
}
