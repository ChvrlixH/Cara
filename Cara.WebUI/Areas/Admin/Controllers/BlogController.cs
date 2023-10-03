using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.WebUI.Areas.Admin.ViewModels.Blog;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(b => b.BCategory).Include(b => b.Author)
                .OrderByDescending(b => b.ModifiedAt).ToListAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var blogs = await _context.Blogs.Include(b=>b.BCategory).Include(b=>b.Author).Include(b => b.BlogTags).ThenInclude(b => b.Tag).FirstOrDefaultAsync(b => b.Id == id);
            if (blogs == null) return NotFound();

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.BCategories = _context.BCategories.AsEnumerable();
            ViewBag.Authors = _context.Authors.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogVM blogVM)
        {
            ViewBag.BCategories = _context.BCategories.AsEnumerable();
            ViewBag.Authors = _context.Authors.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();

            if (!ModelState.IsValid) { return View(blogVM); }

            if (!_context.BCategories.Any(bc => bc.Id == blogVM.BCategoryId) || !_context.Authors.Any(a => a.Id == blogVM.AuthorId))
            { return BadRequest(); }

            if (blogVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Select Photo");
                return View(blogVM);
            }

            if (!blogVM.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
                return View(blogVM);
            }

            if (!blogVM.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "You must be choose image type");
                return View(blogVM);
            }

            var filename = String.Empty;

            try
            {
                filename = await blogVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "blogs");
            }
            catch (Exception)
            {

                return View(blogVM);
            }

            Blog blog = new()
            {
                Title = blogVM.Title,
                Description = blogVM.Description,
                AuthorId = blogVM.AuthorId,
                BCategoryId = blogVM.BCategoryId,
                Photo = filename
            };

            List<BlogTags> tags = new List<BlogTags>();
            foreach (var tagId in blogVM.TagIds)
            {
                BlogTags blogTag = new BlogTags
                {
                    BlogId = blogVM.Id,
                    TagId = tagId
                };
                tags.Add(blogTag);
            }

            blog.BlogTags = tags;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
			ViewBag.BCategories = _context.BCategories.AsEnumerable();
			ViewBag.Authors = _context.Authors.AsEnumerable();
			ViewBag.Tags = _context.Tags.AsEnumerable();

			Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();

            BlogUpdateVM blogUpdateVM = new()
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                AuthorId = blog.AuthorId,
                BCategoryId = blog.BCategoryId,
                ImagePath = blog.Photo
            };

            return View(blogUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogUpdateVM updateVM)
        {
			ViewBag.BCategories = _context.BCategories.AsEnumerable();
			ViewBag.Authors = _context.Authors.AsEnumerable();
			ViewBag.Tags = _context.Tags.AsEnumerable();

			if (!ModelState.IsValid) return View(updateVM);

            if (!_context.BCategories.Any(bc => bc.Id == updateVM.BCategoryId) || !_context.Authors.Any(a => a.Id == updateVM.AuthorId))
            { return BadRequest(); }

            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();

            blog.Title = updateVM.Title;
            blog.Description = updateVM.Description;
            blog.AuthorId = updateVM.AuthorId;
            blog.BCategoryId = updateVM.BCategoryId;
            if (updateVM.Photo != null)
            {
                Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "blogs", blog.Photo);

                if (!updateVM.Photo.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photo", "Image size must be less than 1 mb");
                    return View(updateVM);
                }

                if (!updateVM.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "You must be choose image type");
                    return View(updateVM);
                }

                var filename = String.Empty;

                try
                {
                    filename = await updateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "blogs");
                }
                catch (Exception)
                {

                    return View(updateVM);
                }
                blog.Photo = filename;
            }

            if (updateVM.TagIds != null) 
            {
            var existingTags = await _context.BlogTags.Where(bt => bt.BlogId == id).ToListAsync();
            _context.BlogTags.RemoveRange(existingTags);

                List<BlogTags> tags = new List<BlogTags>();
                foreach (var tagId in updateVM.TagIds)
                {
                    BlogTags blogTag = new BlogTags
                    {
                        BlogId = id,
                        TagId = tagId
                    };
                    tags.Add(blogTag);
                }

                _context.BlogTags.AddRange(tags);

            }    
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeletePost(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog is null) return NotFound();
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "blogs", blog.Photo);

			_context.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
