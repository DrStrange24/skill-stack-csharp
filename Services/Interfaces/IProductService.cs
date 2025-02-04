using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.Models;

namespace SkillStackCSharp.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductDetailsAsync(string id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO product);
        Task<ProductDTO> UpdateProductAsync(string id, UpdateProductDTO product);
        Task<bool> DeleteProductAsync(string id);
    }
}
