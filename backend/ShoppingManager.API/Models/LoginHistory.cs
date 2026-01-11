using System.ComponentModel.DataAnnotations;

namespace ShoppingManager.API.Models
{
    public class LoginHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public virtual User User { get; set; } = null!;
        
        [Required]
        public string IPAddress { get; set; } = string.Empty;
        
        [Required]
        public DateTime LoginTime { get; set; }
        
        public DateTime? LogoutTime { get; set; }
        
        [Required]
        public string UserAgent { get; set; } = string.Empty;
        
        public bool IsSuccessful { get; set; } = true;
    }
}