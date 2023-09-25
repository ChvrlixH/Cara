namespace Cara.WebUI.Areas.Admin.ViewModels.Users
{
    public class AllUserVM
    {
		public string? Id { get; set; }
		public string? Fullname { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public string? Role { get; set; }
		public bool IsActive { get; set; }
	}
}
