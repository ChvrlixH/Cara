using Microsoft.AspNetCore.Identity;

namespace Cara.Core.Entities;

public class AppUser : IdentityUser
{
    public string? Fullname { get; set; }
    public bool IsActive { get; set; }
}
