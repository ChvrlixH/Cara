using Cara.Business.DTOs;
using Cara.Business.Interfaces;
using Cara.Business.Services;
using Cara.Core.Entities;
using Cara.DataAccess.Contexts;
using Cara.WebUI.Areas.Admin.ViewModels.Product;
using Cara.WebUI.Areas.Admin.ViewModels.SmallBanner;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cara.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class ProductController : Controller
{
	private readonly AppDbContext _context;
	private readonly IMailService _mailService;
	private IWebHostEnvironment _env;

	public ProductController(AppDbContext context, IWebHostEnvironment env, IMailService mailService)
	{
		_context = context;
		_env = env;
		_mailService = mailService;
	}

	public async Task<IActionResult> Index()
	{
		var products = await _context.Products.Include(p => p.PCategory).OrderByDescending(b => b.ModifiedAt).ToListAsync();
		return View(products);
	}

	public async Task<IActionResult> Detail(int id)
	{
		var product = await _context.Products.Include(p => p.PCategory).Include(p=>p.ProductImages).Include(p => p.ProductSizes).ThenInclude(p => p.Size).FirstOrDefaultAsync(b => b.Id == id);
		if (product == null) return NotFound();

		return View(product);
	}

	public IActionResult Create()
	{
		ViewBag.PCategories = _context.PCategories.AsEnumerable();
		ViewBag.Sizes = _context.Sizes.AsEnumerable();

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(ProductVM productVM)
	{
		ViewBag.PCategories = _context.PCategories.AsEnumerable();
		ViewBag.Sizes = _context.Sizes.AsEnumerable();

		if (!ModelState.IsValid) { return View(productVM); }

		if (!_context.PCategories.Any(bc => bc.Id == productVM.PCategoryId))
		{ return BadRequest(); }

		if (productVM.Photo == null)
		{
			ModelState.AddModelError("Photo", "Select Photo");
			return View(productVM);
		}

		if (!productVM.Photo.CheckFileSize(100))
		{
			ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
			return View(productVM);
		}

		if (!productVM.Photo.CheckFileFormat("image/"))
		{
			ModelState.AddModelError("Photo", "You must be choose image type");
			return View(productVM);
		}

		var filename = String.Empty;

		try
		{
			filename = await productVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "products", "mainimg");
		}
		catch (Exception)
		{

			return View(productVM);
		}

		Product product = new()
		{
			Name = productVM.Name,
			Price = productVM.Price,
			Description = productVM.Description,
			Rating = productVM.Rating,
			Owner = productVM.Owner,
			PCategoryId = productVM.PCategoryId,
			Photo = filename
		};


		List<ProductSize> sizes = new List<ProductSize>();
		foreach (var sizeId in productVM.SizeIds)
		{
			ProductSize productSize = new ProductSize
			{
				ProductId = productVM.Id,
				SizeId = sizeId
			};
			sizes.Add(productSize);
		}

		product.ProductSizes = sizes;


		string htmlFilePath = Path.Combine(_env.WebRootPath, "admin", "assets", "database", "templates", "BlackFriday.html");

		try
		{
			string htmlContent = System.IO.File.ReadAllText(htmlFilePath);

			List<Subscribe> subscribes = await _context.Subscribes.ToListAsync();
			foreach (Subscribe subscribe in subscribes)
			{
				MailRequestDto mailSendDto = new()
				{
					ToEmail = subscribe.Email,
					Subject = "Product Sale 50%",
					Body = htmlContent
				};

				await _mailService.SendEmailAsync(mailSendDto);
			}
		}
		catch (FileNotFoundException ex)
		{

		}
		catch (Exception ex)
		{

		}

		await _context.Products.AddAsync(product);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Update(int id)
	{
		ViewBag.PCategories = _context.PCategories.AsEnumerable();
		ViewBag.Sizes = _context.Sizes.AsEnumerable();

		Product? product = await _context.Products.FirstOrDefaultAsync(b => b.Id == id);
		if (product == null) return NotFound();

		ProductUpdateVM productUpdateVM = new()
		{
			Id = product.Id,
			Name = product.Name,
			Price = product.Price,
			Description = product.Description,
			Rating = product.Rating,
			Owner = product.Owner,
			PCategoryId = product.PCategoryId,
			ImagePath = product.Photo
		};

		return View(productUpdateVM);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, ProductUpdateVM updateVM)
	{
		ViewBag.PCategories = _context.PCategories.AsEnumerable();
		ViewBag.Sizes = _context.Sizes.AsEnumerable();

		if (!ModelState.IsValid) return View(updateVM);

		if (!_context.PCategories.Any(bc => bc.Id == updateVM.PCategoryId))
		{ return BadRequest(); }

		Product? product = await _context.Products.FirstOrDefaultAsync(b => b.Id == id);
		if (product == null) return NotFound();

		product.Name = updateVM.Name;
		product.Price = updateVM.Price;
		product.Description = updateVM.Description;
		product.Rating = updateVM.Rating;
		product.Owner = updateVM.Owner;
		product.PCategoryId = updateVM.PCategoryId;
		if (updateVM.Photo != null)
		{
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "products", "mainimg", product.Photo);

			if (!updateVM.Photo.CheckFileSize(100))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
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
				filename = await updateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "products", "mainimg");
			}
			catch (Exception)
			{

				return View(updateVM);
			}
			product.Photo = filename;
		}

		if (updateVM.SizeIds != null)
		{
			var existingSizes = await _context.ProductSizes.Where(ps => ps.ProductId == id).ToListAsync();
			_context.ProductSizes.RemoveRange(existingSizes);

			List<ProductSize> sizes = new List<ProductSize>();
			foreach (var sizeId in updateVM.SizeIds)
			{
				ProductSize blogTag = new ProductSize
				{
					ProductId = id,
					SizeId = sizeId
				};
				sizes.Add(blogTag);
			}

			_context.ProductSizes.AddRange(sizes);

		}
		await _context.SaveChangesAsync();

		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var product = await _context.Products.FirstOrDefaultAsync(b => b.Id == id);

		if (product is null) return NotFound();

		return View(product);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[ActionName(nameof(Delete))]
	public async Task<IActionResult> DeletePost(int id)
	{
		var product = await _context.Products.FirstOrDefaultAsync(b => b.Id == id);
		if (product is null) return NotFound();
		Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "products", "mainimg", product.Photo);

		_context.Remove(product);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
}
