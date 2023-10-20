using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Comment;

public class CommentVM
{
	[Required]
	public IFormFile? Photo { get; set; }
	[Required, MaxLength(40)]
	public string? Fullname { get; set; }
	[Required, MaxLength(50)]
	public string? Profession { get; set; }
	[Required, MaxLength(130)]
	public string? Description { get; set; }
}
