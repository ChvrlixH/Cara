using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Team;

public class TeamVM
{
    [Required,MaxLength(40)]
    public string? Fullname { get; set; }
    [Required]
    public IFormFile? Photo { get; set; }
}
