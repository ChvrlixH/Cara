using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Blog;

public class BlogUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string? Title { get; set; }
    public IFormFile? Photo { get; set; }
    [Required]
    public string? Description { get; set; }
    public int BCategoryId { get; set; }
    public int AuthorId { get; set; }
    public int[]? TagIds { get; set; }
    public string? ImagePath { get; set; }
}
