using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.Models;
using SkillStackCSharp.Repositories.Interfaces;
using SkillStackCSharp.Services.Interfaces;


namespace SkillStackCSharp.Services.Implementations
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

        public async Task<Product> GetProductDetailsAsync(string id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<Product> UpdateProductAsync(string id, UpdateProductDTO updatedProduct)
        {
            var product = await GetProductDetailsAsync(id);

            if (product == null)
                return null;

            // Update the product properties
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
