using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.Models;

namespace SkillStackCSharp.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductDetailsAsync(string id);
        Task CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(string id, UpdateProductDTO product);
        Task DeleteProductAsync(Product product);
    }
}
