﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync();
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
