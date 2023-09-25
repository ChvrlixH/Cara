using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.ViewModels;

public class ResetPasswordViewModel
{
    [Required, MaxLength(100), DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string? ConfirmNewPassword { get; set; }
}
