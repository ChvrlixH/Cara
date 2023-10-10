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

        public async Task<IActionResult> Index()
        {
			IQueryable<Blog> blogs = _context.Blogs.AsQueryable();

			BlogViewModel blogViewModel = new()
            {
                Blogs = await _context.Blogs.Where(b=>!b.BCategory.IsDeleted).ToListAsync(),
                BCategories = await _context.BCategories.Include(b=>b.Blogs).Where(b=>!b.IsDeleted).ToListAsync(),
                Authors = await _context.Authors.ToListAsync(),
                Tags = await _context.Tags.ToListAsync(),
                BlogBanners = await _context.BlogBanners.ToListAsync()
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

			ViewBag.BlogsDetailCount = _context.Blogs.Count();

			return View(blogViewModel);
        }

        public IActionResult FilterProducts(int? categoryId)
        {
            var categories = _context.BCategories.ToList();
            var blogs = categoryId == null ? _context.Blogs.ToList() : _context.Blogs.Where(b => b.BCategoryId == categoryId).ToList();


            var viewModel = new BlogViewModel
            {
                BCategories = categories,
                Blogs = blogs
            };

            return View("Index", viewModel);
        }

        public IActionResult LoadMoreDetail(int skip)
        {
            var blogs = _context.Blogs.Include(b => b.BCategory).OrderByDescending(b => b.ModifiedAt).Skip(skip).Take(3).ToList();

			return PartialView("_BlogDetailPartial", blogs);
        }

    }
}
