using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.Blog;

public class BlogVM
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string? Title { get; set; }
    [Required]
    public IFormFile? Photo { get; set; }
    [Required] 
    public string? Description { get; set; }
    public int BCategoryId { get; set; }
    public int AuthorId { get; set; }
    [Required]
    public int[]? TagIds { get; set; }
}
