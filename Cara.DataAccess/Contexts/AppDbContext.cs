using Cara.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cara.DataAccess.Contexts
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
		{ 
		}

		public DbSet<HeroItem> HeroItems { get; set; } = null!;
		public DbSet<MainBannerItem> MainBannerItems { get; set; } = null!;
		public DbSet<Feature> Features { get; set; } = null!;
		public DbSet<SmallBannerItem> SmallBannerItems { get; set; } = null!;
		public DbSet<Subscribe> Subscribes { get; set; } = null!;
		public DbSet<PCategory> PCategories { get; set; } = null!;
	}
}
