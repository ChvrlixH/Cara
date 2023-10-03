using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            BlogViewModel blogViewModel = new()
            {
                Blogs = _context.Blogs.AsNoTracking(),
                BCategories = _context.BCategories.AsNoTracking(),
                Authors = _context.Authors.AsNoTracking(),
                Tags = _context.Tags.AsNoTracking(),
                BlogBanners = _context.BlogBanners.AsNoTracking()
            };
            return View(blogViewModel);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var blog = _context.Blogs
       .Include(b => b.BCategory)
       .Include(b => b.Author)
       .Include(b => b.BlogTags)
       .ThenInclude(b => b.Tag)
       .FirstOrDefault(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            BlogViewModel blogViewModel = new()
            {
                Blogs = new List<Blog> { blog },
                BCategories = _context.BCategories.AsNoTracking(),
                Authors = _context.Authors.AsNoTracking(),
                Tags = blog.BlogTags.Select(bt => bt.Tag).ToList()
            };
            return View(blogViewModel);
        }

        public IActionResult Category(int? categoryId)
        {
            if (categoryId == null)
            {
                // Kategori belirtilmediyse, tüm blogları göstermek için tüm blogları alın
                var allBlogs = _context.Blogs.ToList();

                var blogViewModel = new BlogViewModel
                {
                    Blogs = allBlogs,
                    BCategories = _context.BCategories.ToList(),
                    // Diğer özellikleri de ekleyebilirsiniz
                };

                return View("Index", blogViewModel); // Index.cshtml görünümünü döndürün
            }
            else
            {
                // Belirli bir kategoriye ait blogları alın
                var blogsInCategory = _context.Blogs
                    .Where(b => b.BCategoryId == categoryId)
                    .ToList();

                var blogViewModel = new BlogViewModel
                {
                    Blogs = blogsInCategory,
                    BCategories = _context.BCategories.ToList(),
                    // Diğer özellikleri de ekleyebilirsiniz
                };

                return View("Index", blogViewModel);
            }

        }

    }
}
