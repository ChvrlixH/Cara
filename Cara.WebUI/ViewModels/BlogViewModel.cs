using Cara.Core.Entities;
using Cara.Core.Entities.HeadBanners;

namespace Cara.WebUI.ViewModels;

public class BlogViewModel
{
	public IEnumerable<Blog> Blogs { get; set; } = null!;
	public IEnumerable<BCategory> BCategories { get; set; } = null!;
	public IEnumerable<Author> Authors { get; set; } = null!;
	public IEnumerable<Tag> Tags { get; set; } = null!;
	public IEnumerable<BlogBanner> BlogBanners { get; set; } = null!;

}
