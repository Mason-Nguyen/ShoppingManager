using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ShoppingManager.API.DTOs;
using ShoppingManager.API.Services;
using ShoppingManager.API.Models;

namespace ShoppingManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var ipAddress = GetClientIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();
            
            var result = await _authService.LoginAsync(loginDto, ipAddress, userAgent);
            
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });
            
            return Ok(result);
        }
        
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.RegisterAsync(registerDto);
            
            if (result == null)
                return BadRequest(new { message = "User with this email already exists" });
            
            return Ok(result);
        }
        
        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Additional validation for admin user creation
            if (!Enum.IsDefined(typeof(UserRole), registerDto.Role))
                return BadRequest(new { message = "Invalid role specified" });
            
            var result = await _authService.RegisterAsync(registerDto);
            
            if (result == null)
                return BadRequest(new { message = "User with this email already exists" });
            
            return Ok(new { 
                message = $"User created successfully with role {registerDto.Role}",
                user = result.User 
            });
        }
        
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAvailableRoles()
        {
            var roles = Enum.GetValues(typeof(UserRole))
                           .Cast<UserRole>()
                           .Select(r => new { 
                               Value = (int)r, 
                               Name = r.ToString(),
                               Description = GetRoleDescription(r)
                           })
                           .ToList();
            
            return Ok(roles);
        }
        
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
            
            if (!result)
                return BadRequest(new { message = "Current password is incorrect" });
            
            return Ok(new { message = "Password changed successfully" });
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            
            if (!result)
                return BadRequest(new { message = "User not found" });
            
            return Ok(new { message = "Password reset email sent" });
        }
        
        [HttpPost("confirm-reset-password")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordDto confirmResetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.ConfirmResetPasswordAsync(confirmResetPasswordDto);
            
            if (!result)
                return BadRequest(new { message = "Invalid or expired reset token" });
            
            return Ok(new { message = "Password reset successfully" });
        }
        
        [HttpPost("admin-update-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUpdatePassword([FromBody] AdminUpdatePasswordDto adminUpdatePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.AdminUpdatePasswordAsync(adminUpdatePasswordDto);
            
            if (!result)
                return BadRequest(new { message = "User not found" });
            
            return Ok(new { message = "Password updated successfully" });
        }
        
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var ipAddress = GetClientIpAddress();
            
            await _authService.LogoutAsync(userId, ipAddress);
            
            return Ok(new { message = "Logged out successfully" });
        }
        
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"GetCurrentUser - Role claim value: {roleClaimValue}");
            
            var user = new
            {
                Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                Email = User.FindFirst(ClaimTypes.Email)!.Value,
                FirstName = User.FindFirst("firstName")!.Value,
                LastName = User.FindFirst("lastName")!.Value,
                Role = roleClaimValue
            };
            
            Console.WriteLine($"GetCurrentUser - Returning user: {System.Text.Json.JsonSerializer.Serialize(user)}");
            
            return Ok(user);
        }
        
        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            
            if (Request.Headers.ContainsKey("X-Real-IP"))
                ipAddress = Request.Headers["X-Real-IP"].FirstOrDefault();
            
            return ipAddress ?? "Unknown";
        }
        
        private string GetRoleDescription(UserRole role)
        {
            return role switch
            {
                UserRole.Admin => "Full system access with user management capabilities",
                UserRole.Requester => "Can create and submit shopping requests",
                UserRole.Approver => "Can review and approve shopping requests",
                UserRole.Receiver => "Can receive and confirm delivered items",
                UserRole.Purchase => "Can process approved requests and make purchases",
                UserRole.User => "Basic user with limited access",
                _ => "Standard user access"
            };
        }
    }
}