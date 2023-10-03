using Cara.Business.Helpers;
using Cara.Business.Interfaces;
using Cara.Business.Services;
using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.DataAccess.Repositories.Implementations;
using Cara.DataAccess.Repositories.Implementations.HeadBanners;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.DataAccess.Repositories.Interfaces.IHeadBanners;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddControllersWithViews();
//var constr = builder.Configuration["ConnectionStrings:Default"];
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser,IdentityRole>(options=>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit= true;
    options.Password.RequireNonAlphanumeric= true;
    options.Password.RequireLowercase= true;
    options.Password.RequireUppercase= true;

    options.User.RequireUniqueEmail= true;

    options.Lockout.MaxFailedAccessAttempts = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.AllowedForNewUsers= true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromHours(10);
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Auth/Login";
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IHeroItemRepository, HeroItemRepository>();
builder.Services.AddScoped<IMainBannerRepository, MainBannerRepository>();
builder.Services.AddScoped<ISmallBannerRepository, SmallBannerRepository>();
builder.Services.AddScoped<IPCategoryRepository, PCategoryRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBCategoryRepository, BCategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ISubscribeRepository, SubscribeRepository>();
builder.Services.AddScoped<IShopBannerRepository, ShopBannerRepository>();
builder.Services.AddScoped<IBlogBannerRepository, BlogBannerRepository>();
builder.Services.AddScoped<IAboutBannerRepository, AboutBannerRepository>();
builder.Services.AddScoped<IContactBannerRepository, ContactBannerRepository>();
builder.Services.AddScoped<ICartBannerRepository, CartBannerRepository>();

builder.Services.AddTransient<IMailService, MailService>();

var app = builder.Build();



//handle http request
app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name:"default",
    pattern:"{controller=Home}/{action=Index}/{Id?}"
);


app.Run();
