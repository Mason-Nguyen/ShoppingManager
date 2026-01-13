using Microsoft.EntityFrameworkCore;
using ShoppingManager.API.Data;
using ShoppingManager.API.DTOs;
using ShoppingManager.API.Models;

namespace ShoppingManager.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<ProductDto?> GetProductByCodeAsync(string code);
        Task<ProductDto?> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> ProductExistsAsync(Guid id);
        Task<bool> ProductCodeExistsAsync(string code, Guid? excludeId = null);
    }
    
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();
            
            return products.Select(MapToDto);
        }
        
        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            return product != null ? MapToDto(product) : null;
        }
        
        public async Task<ProductDto?> GetProductByCodeAsync(string code)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Code == code);
            return product != null ? MapToDto(product) : null;
        }
        
        public async Task<ProductDto?> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Check if product code already exists
            if (await ProductCodeExistsAsync(createProductDto.Code))
                return null;
            
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Code = createProductDto.Code,
                Name = createProductDto.Name,
                Unit = createProductDto.Unit,
                RefPrice = createProductDto.RefPrice,
                Image = createProductDto.Image,
                Description = createProductDto.Description,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return MapToDto(product);
        }
        
        public async Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;
            
            // Check if product code already exists (excluding current product)
            if (await ProductCodeExistsAsync(updateProductDto.Code, id))
                return null;
            
            product.Code = updateProductDto.Code;
            product.Name = updateProductDto.Name;
            product.Unit = updateProductDto.Unit;
            product.RefPrice = updateProductDto.RefPrice;
            product.Image = updateProductDto.Image;
            product.Description = updateProductDto.Description;
            product.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return MapToDto(product);
        }
        
        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<bool> ProductExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
        
        public async Task<bool> ProductCodeExistsAsync(string code, Guid? excludeId = null)
        {
            var query = _context.Products.Where(p => p.Code == code);
            
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);
            
            return await query.AnyAsync();
        }
        
        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Unit = product.Unit,
                RefPrice = product.RefPrice,
                Image = product.Image,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }
}