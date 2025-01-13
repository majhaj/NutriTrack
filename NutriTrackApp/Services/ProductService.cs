using Microsoft.EntityFrameworkCore;
using NutriTrackData.Entities;
using NutriTrack.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NutriTrack.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with id {product.Id} not found");
            }

            existingProduct.Name = product.Name;
            existingProduct.Calories = product.Calories;
            existingProduct.Protein = product.Protein;
            existingProduct.Carbs = product.Carbs;
            existingProduct.Fat = product.Fat;

            await _context.SaveChangesAsync();
        }

    }
}
