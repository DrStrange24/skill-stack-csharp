using AutoMapper;
using SkillStackCSharp.DTOs.ProductDTOs;
using SkillStackCSharp.DTOs.UserDTOs;
using SkillStackCSharp.Models;
using SkillStackCSharp.Repositories.Interfaces;
using SkillStackCSharp.Services.Interfaces;


namespace SkillStackCSharp.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return productDTOs;
        }

        public async Task<ProductDTO> GetProductDetailsAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return null;

            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            _productRepository.AddProduct(product);
            await _productRepository.SaveChangesAsync();
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        public async Task<ProductDTO> UpdateProductAsync(string id, UpdateProductDTO updatedProduct)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return null;

            _mapper.Map(updatedProduct, product);

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChangesAsync();

            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return false;

            _productRepository.RemoveProduct(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }
    }
}
