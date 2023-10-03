using Cara.Core.Entities;
using Cara.Core.Entities.Common;
using Cara.Core.Entities.HeadBanners;
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
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<Size> Sizes { get; set; } = null!;
        public DbSet<ProductSize> ProductSizes { get; set; } = null!;
		public DbSet<PCategory> PCategories { get; set; } = null!;
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<BCategory> BCategories { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<BlogTags> BlogTags { get; set; } = null!;
        public DbSet<ShopBanner> ShopBanners { get; set; } = null!;
        public DbSet<BlogBanner> BlogBanners { get; set; } = null!;
        public DbSet<AboutBanner> AboutBanners { get; set; } = null!;
        public DbSet<ContactBanner> ContactBanners { get; set; } = null!;
        public DbSet<CartBanner> CartBanners { get; set; } = null!;


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PCategory>().HasQueryFilter(pc => !pc.IsDeleted);
			modelBuilder.Entity<BCategory>().HasQueryFilter(pc => !pc.IsDeleted);
			base.OnModelCreating(modelBuilder);
		}

	}
}
