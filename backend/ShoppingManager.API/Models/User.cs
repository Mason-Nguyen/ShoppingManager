using System.ComponentModel.DataAnnotations;

namespace ShoppingManager.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public string? LastLoginIP { get; set; }
        
        public string? ResetToken { get; set; }
        
        public DateTime? ResetTokenExpiry { get; set; }
        
        public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();
    }
    
    public enum UserRole
    {
        User = 0,
        Admin = 1,
        Requester = 2,
        Approver = 3,
        Receiver = 4,
        Purchase = 5
    }
}