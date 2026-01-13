using System.ComponentModel.DataAnnotations;

namespace ShoppingManager.API.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal RefPrice { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class CreateProductDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Unit { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue, ErrorMessage = "Reference price must be a positive number")]
        public decimal RefPrice { get; set; } = 0;
        
        [StringLength(255)]
        public string? Image { get; set; }
        
        public string? Description { get; set; }
    }
    
    public class UpdateProductDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Unit { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue, ErrorMessage = "Reference price must be a positive number")]
        public decimal RefPrice { get; set; }
        
        [StringLength(255)]
        public string? Image { get; set; }
        
        public string? Description { get; set; }
    }
}