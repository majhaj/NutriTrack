using NutriTrackData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NutriTrack.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
    }
}
