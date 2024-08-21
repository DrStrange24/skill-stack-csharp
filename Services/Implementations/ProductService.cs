using WebApp.Models;
using WebApp.Repositories.Interfaces;
using WebApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductDetailsAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChangesAsync(); // Save changes to the database
        }

        public async Task DeleteProductAsync(Product product)
        {
            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesAsync(); // Save changes to the database
        }
    }
}
