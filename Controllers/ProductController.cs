using backend.Models;
using backend.Services.Implementations;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet(Name = "GetProducts")]
        public IEnumerable<ProductController> Get()
        {
            return null;
        }

        [HttpPost(Name = "PostProduct")]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");

            // Insert the product into your data store (e.g., database)
            await _productService.CreateProductAsync(product);

            // For now, we just log the received product
            _logger.LogInformation($"Received product: {product.Name}, Price: {product.Price}");

            // Return a response indicating that the product was created successfully
            return CreatedAtRoute("GetProducts", new { id = product.Id }, product);
        }
    }
}
