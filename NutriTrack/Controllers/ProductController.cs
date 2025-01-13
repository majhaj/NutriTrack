using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Services;
using NutriTrackData.Entities;

[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? name, double? minCalories, double? maxCalories)
    {
        var products = await _productService.GetAllProductsAsync();

        if (!string.IsNullOrEmpty(name))
        {
            products = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (minCalories.HasValue)
        {
            products = products.Where(p => p.Calories >= minCalories.Value).ToList();
        }

        if (maxCalories.HasValue)
        {
            products = products.Where(p => p.Calories <= maxCalories.Value).ToList();
        }

        return View(products);
    }



    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound($"Product with id {id} doesn't exist.");
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound(value: "Product not found.");
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Calories,Protein,Carbs,Fat")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
            }
            catch (Exception)
            {
                if (!await ProductExistsAsync(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Calories,Protein,Carbs,Fat")] Product product)
    {
        var products = await _productService.GetAllProductsAsync();
        if (products.Any(p => p.Name == product.Name))
        {
            ModelState.AddModelError("Name", "The product name must be unique.");
        }
        if (ModelState.IsValid)
        {
            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }


    private async Task<bool> ProductExistsAsync(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product != null;
    }
}
