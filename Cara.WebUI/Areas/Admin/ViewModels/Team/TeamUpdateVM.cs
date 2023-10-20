using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Team;

public class TeamUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(40)]
    public string? Fullname { get; set; }
    public IFormFile? Photo { get; set; }
    public string? ImagePath { get; set; }
}
