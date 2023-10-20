using System.ComponentModel.DataAnnotations;

namespace Cara.WebUI.Areas.Admin.ViewModels.AuthorBlog
{
	public class AuthorUpdateVM
	{
		public int Id { get; set; }
		[Required, MaxLength(85)]
		public string? Fullname { get; set; }
		public IFormFile? Photo { get; set; }
		[Required, MaxLength(75)]
		public string? Profession { get; set; }
		[Required, DataType(DataType.PhoneNumber)]
		public string? Phone { get; set; }
		[Required, MaxLength(256), DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		public string? ImagePath { get; set; }
	}
}
