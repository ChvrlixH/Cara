using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cara.WebUI.Controllers
{
	public class ShopController : Controller
	{
		private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }

		public async Task<IActionResult> Index()
		{
			IQueryable<Product> products = _context.Products.AsQueryable();

			ViewBag.ProductsCount = await products.CountAsync();

			ShopViewModel shopViewModel = new()
			{
				Products = await products.ToListAsync(),
				PCategories = await _context.PCategories.Include(c => c.Products).Where(p => !p.IsDeleted).ToListAsync(),
				ShopBanners = await _context.ShopBanners.ToListAsync(),
				ProductImages = await _context.ProductImages.ToListAsync(),
				Sizes = await _context.Sizes.ToListAsync()
			};

			ViewBag.ProductCount = _context.Products.Count();

			return View(shopViewModel);
		}
		public async Task<IActionResult> Detail(int? productId)
		{
			if (productId == null)
			{
				return NotFound(); 
			}

			var product = _context.Products.Include(p=>p.PCategory)
				.Include(p=>p.ProductImages).Include(p=>p.ProductSizes)
				.ThenInclude(ps=>ps.Size).FirstOrDefault(p => p.Id == productId);

			var productImages = _context.ProductImages.Where(pi => pi.ProductId == productId).ToList();

			if (product == null)
			{
				return NotFound(); 
			}

			var viewModel = new ShopViewModel
			{
				Products = new List<Product> { product },
				ProductImages = productImages,
				Sizes = product.ProductSizes.Select(ps=>ps.Size).ToList()
			};

			return View(viewModel);
		}


		public IActionResult FilterProducts(int categoryId)
		{
			var categories = _context.PCategories.ToList();
			var products = _context.Products.Where(p => p.PCategoryId == categoryId).ToList();

			var viewModel = new ShopViewModel
			{
				PCategories = categories,
				Products = products
			};

			return View("Index", viewModel);
		}

		public IActionResult LoadMoreProduct(int skip, int? categoryId)
		{
			var query = _context.Products.AsQueryable();

			if (categoryId.HasValue)
			{
				query = query.Where(p => p.PCategoryId == categoryId);
			}

			var products = query.Skip(skip).Take(8).ToList();

			return PartialView("_PLoadMorePartial", products);
		}
	}
}
