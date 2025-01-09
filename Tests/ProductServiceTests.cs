using Microsoft.EntityFrameworkCore;
using NutriTrackData.Entities;
using NutriTrack.Data;
using NutriTrack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ProductServiceTests
{
    private readonly ProductService _productService;
    private readonly ApplicationDbContext _context;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _productService = new ProductService(_context);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        _context.Products.AddRange(
            new Product { Id = 1, Name = "Apple", Calories = 50 },
            new Product { Id = 2, Name = "Banana", Calories = 90 }
        );
        await _context.SaveChangesAsync();

        var result = await _productService.GetAllProductsAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Apple");
        Assert.Contains(result, p => p.Name == "Banana");
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsCorrectProduct()
    {
        var product = new Product { Id = 1, Name = "Apple", Calories = 50 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var result = await _productService.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Apple", result.Name);
        Assert.Equal(50, result.Calories);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNull_WhenProductNotFound()
    {
        var result = await _productService.GetProductByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddProductAsync_AddsProductToDatabase()
    {
        var product = new Product { Id = 1, Name = "Orange", Calories = 60 };

        await _productService.AddProductAsync(product);

        var addedProduct = await _context.Products.FindAsync(1);
        Assert.NotNull(addedProduct);
        Assert.Equal("Orange", addedProduct.Name);
        Assert.Equal(60, addedProduct.Calories);
    }

    [Fact]
    public async Task UpdateProductAsync_UpdatesExistingProduct()
    {
        var product = new Product { Id = 1, Name = "Orange", Calories = 60 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        product.Name = "Updated Orange";
        product.Calories = 80;
        await _productService.UpdateProductAsync(product);

        var updatedProduct = await _context.Products.FindAsync(1);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Orange", updatedProduct.Name);
        Assert.Equal(80, updatedProduct.Calories);
    }

    [Fact]
    public async Task UpdateProductAsync_DoesNotAddNewProduct()
    {
        var product = new Product { Id = 1, Name = "Orange", Calories = 60 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var updatedProduct = new Product { Id = 2, Name = "New Product", Calories = 100 };

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await _productService.UpdateProductAsync(updatedProduct);
        });

        Assert.Null(await _context.Products.FindAsync(2));
    }

}
