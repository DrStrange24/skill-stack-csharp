using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // Get all products
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // Get a product by Id
        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductDetailsAsync(id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            return Ok(product);
        }

        // Create a new product (already provided but updated to async)
        [HttpPost(Name = "PostProduct")]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");

            await _productService.CreateProductAsync(product);

            _logger.LogInformation($"Created product: {product.Name}, Price: {product.Price}");

            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // Update an existing product
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        public async Task<IActionResult> Update(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest("Updated product is null.");

            var product = await _productService.GetProductDetailsAsync(id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            // Update the product properties
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            await _productService.UpdateProductAsync(product);

            return Ok(product);
        }

        // Delete a product by Id
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductDetailsAsync(id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            await _productService.DeleteProductAsync(product);

            _logger.LogInformation($"Deleted product with Id = {id}");

            return NoContent();
        }
    }
}
