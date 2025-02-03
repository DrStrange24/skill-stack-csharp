using SkillStackCSharp.Models;
using SkillStackCSharp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SkillStackCSharp.DTOs.ProductDTOs;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetProductDetailsAsync(id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            return Ok(product);
        }

        // Create a new product (already provided but updated to async)
        [HttpPost(Name = "PostProduct")]
        public async Task<IActionResult> Post([FromBody] CreateProductDTO productDTO)
        {
            if (productDTO == null)
                return BadRequest("Product data is null.");

            var product = await _productService.CreateProductAsync(productDTO);

            _logger.LogInformation($"Created product: {product.Name}, Price: {product.Price}");

            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // Update an existing product
        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDTO updatedProduct)
        {
            if (updatedProduct == null)
                return BadRequest("Updated product is null.");

            var result = await _productService.UpdateProductAsync(id,updatedProduct);

            if (result == null)
                return NotFound($"Product with Id = {id} not found.");

            return Ok(result);
        }

        // Delete a product by Id
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound($"Id is Empty");

            var result = await _productService.DeleteProductAsync(id);

            if (!result)
                return NotFound($"Product with Id = {id} not found.");

            _logger.LogInformation($"Deleted product with Id = {id}");

            return NoContent();
        }
    }
}
