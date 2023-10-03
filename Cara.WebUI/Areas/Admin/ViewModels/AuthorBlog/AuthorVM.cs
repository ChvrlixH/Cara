using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.AuthorBlog
{
	public class AuthorVM
	{
		[Required, MaxLength(85)]
		public string? Fullname { get; set; }
		[Required]
		public IFormFile? Photo { get; set; }
		[Required, MaxLength(75)]
		public string? Profession { get; set; }
		[Required, DataType(DataType.PhoneNumber)]
		public int Phone { get; set; }
		[Required, MaxLength(256), DataType(DataType.PhoneNumber)]
		public string? Email { get; set; }
	}
}
