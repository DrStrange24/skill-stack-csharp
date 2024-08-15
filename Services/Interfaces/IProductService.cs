using backend.Models;
using backend.Repositories.Interfaces;

namespace backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductDetailsAsync(int id);
        Task CreateProductAsync(Product product);
    }
}
