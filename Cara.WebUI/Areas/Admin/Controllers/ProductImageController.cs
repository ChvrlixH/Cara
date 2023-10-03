using Cara.Core.Entities;
using Cara.DataAccess.Repositories.Interfaces;
using Cara.WebUI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Cara.WebUI.Areas.Admin.ViewModels.ProductImage;
using Microsoft.AspNetCore.Authorization;
using Cara.DataAccess.Contexts;

namespace Cara.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class ProductImageController : Controller
{
	private readonly AppDbContext _context;
	private IProductImageRepository _repository;
	private readonly IWebHostEnvironment _env;
	public ProductImageController(IProductImageRepository repository, IWebHostEnvironment env, AppDbContext context)
	{
		_repository = repository;
		_env = env;
		_context = context;
	}

	public async Task<IActionResult> Index()
	{
		var productImage = await _repository.ListIncludeAsync();
		return View(productImage);
	}

	public IActionResult Create()
	{
		ViewBag.Products = _context.Products.AsEnumerable();
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(ProductImageVM productImageVM)
	{
		ViewBag.Products = _context.Products.AsEnumerable();

		if (!ModelState.IsValid)
		{
			return View(productImageVM);
		}

		if (!_context.Products.Any(a => a.Id == productImageVM.ProductId))
		{ return BadRequest(); }

		if (productImageVM.Photo == null)
		{
			ModelState.AddModelError("Photo", "Select Photo");
			return View(productImageVM);
		}

		if (!productImageVM.Photo.CheckFileSize(500))
		{
			ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
			return View(productImageVM);
		}

		if (!productImageVM.Photo.CheckFileFormat("image/"))
		{
			ModelState.AddModelError("Photo", "You must be choose image type");
			return View(productImageVM);
		}

		var filename = String.Empty;

		try
		{
			filename = await productImageVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "products", "productimg");
		}
		catch (Exception)
		{

			return View(productImageVM);
		}

		ProductImage productImage = new()
		{
			ProductId = productImageVM.ProductId,
			Photo = filename
		};


		await _repository.CreateAsync(productImage);
		await _repository.SaveAsync();
		return RedirectToAction(nameof(Index));
	}


	public async Task<IActionResult> Detail(int id)
	{
		var productImage = await _repository.FirstInclude(id);
		if (productImage == null) return NotFound();
		return View(productImage);
	}

	public async Task<IActionResult> Update(int id)
	{
		ViewBag.Products = _context.Products.AsEnumerable();

		var productImage = await _repository.GetAsync(id);
		if (productImage == null) return NotFound();

		PImageUpdateVM productImageUpdateVM = new()
		{
			ProductId = productImage.ProductId,
			ImagePath = productImage.Photo
		};


		return View(productImageUpdateVM);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, PImageUpdateVM productImageUpdateVM)
	{
		ViewBag.Products = _context.Products.AsEnumerable();

		if (id != productImageUpdateVM.Id) return BadRequest();
		if (!ModelState.IsValid) return View(productImageUpdateVM);
		var productImage = await _repository.GetAsync(id);
		if (productImage == null) return NotFound();

		if (!_context.Products.Any(a => a.Id == productImageUpdateVM.ProductId))
		{ return BadRequest(); }

		productImage.ProductId = productImageUpdateVM.ProductId;
		if (productImageUpdateVM.Photo != null)
		{
			Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "products", "productimg", productImage.Photo);

			if (!productImageUpdateVM.Photo.CheckFileSize(500))
			{
				ModelState.AddModelError("Photo", "Image size must be less than 500 kb");
				return View(productImageUpdateVM);
			}

			if (!productImageUpdateVM.Photo.CheckFileFormat("image/"))
			{
				ModelState.AddModelError("Photo", "You must be choose image type");
				return View(productImageUpdateVM);
			}
			var filename = String.Empty;

			try
			{
				filename = await productImageUpdateVM.Photo.CopyFileAsync(_env.WebRootPath, "admin", "assets", "database", "products", "productimg");
			}
			catch (Exception)
			{

				return View(productImageUpdateVM);
			}
			productImage.Photo = filename;
		}



		_repository.Update(productImage);
		await _repository.SaveAsync();
		return RedirectToAction(nameof(Index));
	}


	public async Task<IActionResult> Delete(int id)
	{
		var productImage = await _repository.GetAsync(id);
		if (productImage == null) return NotFound();
		return View(productImage);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[ActionName("Delete")]

	public async Task<IActionResult> DeletePost(int id)
	{
		var productImage = await _repository.GetAsync(id);
		if (productImage == null) return NotFound();
		Helper.DeleteFile(_env.WebRootPath, "admin", "assets", "database", "products", "productimg", productImage.Photo);

		_repository.Delete(productImage);
		await _repository.SaveAsync();
		return RedirectToAction(nameof(Index));
	}
}
