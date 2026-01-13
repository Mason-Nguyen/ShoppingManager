using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingManager.API.DTOs;
using ShoppingManager.API.Services;

namespace ShoppingManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product == null)
                return NotFound(new { message = "Product not found" });
            
            return Ok(product);
        }
        
        [HttpGet("by-code/{code}")]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> GetProductByCode(string code)
        {
            var product = await _productService.GetProductByCodeAsync(code);
            
            if (product == null)
                return NotFound(new { message = "Product not found" });
            
            return Ok(product);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var product = await _productService.CreateProductAsync(createProductDto);
            
            if (product == null)
                return BadRequest(new { message = "Product with this code already exists" });
            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var product = await _productService.UpdateProductAsync(id, updateProductDto);
            
            if (product == null)
            {
                if (!await _productService.ProductExistsAsync(id))
                    return NotFound(new { message = "Product not found" });
                
                return BadRequest(new { message = "Product with this code already exists" });
            }
            
            return Ok(product);
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            
            if (!result)
                return NotFound(new { message = "Product not found" });
            
            return Ok(new { message = "Product deleted successfully" });
        }
        
        [HttpGet("check-code/{code}")]
        [Authorize(Roles = "Admin,Purchase")]
        public async Task<IActionResult> CheckProductCode(string code, [FromQuery] Guid? excludeId = null)
        {
            var exists = await _productService.ProductCodeExistsAsync(code, excludeId);
            return Ok(new { exists });
        }
    }
}