using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services.Implementations
{
    // Services/ProductService.cs
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateProductAsync(Product product)
        {
            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task<Product> GetProductDetailsAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }
    }

}
