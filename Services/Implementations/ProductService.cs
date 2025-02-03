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

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            });
            return productDTOs;
        }

        public async Task<ProductDTO> GetProductDetailsAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return null;

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
            return productDTO;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            var product = new Product
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
            };

            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesAsync();

            var productDTO = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return productDTO;
        }

        public async Task<ProductDTO> UpdateProductAsync(string id, UpdateProductDTO updatedProduct)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return null;

            // Update the product properties
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChangesAsync();

            var productDTO = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return productDTO;
        }

        public async Task DeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return;
            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesAsync();
        }
    }
}
