using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShoppingManager.API.Data;
using ShoppingManager.API.DTOs;
using ShoppingManager.API.Models;

namespace ShoppingManager.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, string ipAddress, string userAgent);
        Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<bool> ConfirmResetPasswordAsync(ConfirmResetPasswordDto confirmResetPasswordDto);
        Task LogoutAsync(int userId, string ipAddress);
    }
    
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto, string ipAddress, string userAgent)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            
            if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                // Log failed login attempt
                if (user != null)
                {
                    var failedLogin = new LoginHistory
                    {
                        UserId = user.Id,
                        IPAddress = ipAddress,
                        LoginTime = DateTime.UtcNow,
                        UserAgent = userAgent,
                        IsSuccessful = false
                    };
                    _context.LoginHistories.Add(failedLogin);
                    await _context.SaveChangesAsync();
                }
                return null;
            }
            
            // Update user last login info
            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIP = ipAddress;
            
            // Log successful login
            var loginHistory = new LoginHistory
            {
                UserId = user.Id,
                IPAddress = ipAddress,
                LoginTime = DateTime.UtcNow,
                UserAgent = userAgent,
                IsSuccessful = true
            };
            
            _context.LoginHistories.Add(loginHistory);
            await _context.SaveChangesAsync();
            
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                }
            };
        }
        
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return null;
            
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Role = registerDto.Role,
                IsActive = registerDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            var token = GenerateJwtToken(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                }
            };
        }
        
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                return false;
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null)
                return false;
            
            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            
            await _context.SaveChangesAsync();
            
            // In a real application, you would send an email with the reset token
            // For demo purposes, we'll just return true
            return true;
        }
        
        public async Task<bool> ConfirmResetPasswordAsync(ConfirmResetPasswordDto confirmResetPasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.ResetToken == confirmResetPasswordDto.Token && 
                u.ResetTokenExpiry > DateTime.UtcNow);
            
            if (user == null)
                return false;
            
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(confirmResetPasswordDto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task LogoutAsync(int userId, string ipAddress)
        {
            var lastLogin = await _context.LoginHistories
                .Where(lh => lh.UserId == userId && lh.LogoutTime == null)
                .OrderByDescending(lh => lh.LoginTime)
                .FirstOrDefaultAsync();
            
            if (lastLogin != null)
            {
                lastLogin.LogoutTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);
            
            // Debug logging
            Console.WriteLine($"Generating JWT for user: {user.Email}, Role: {user.Role}, Role.ToString(): {user.Role.ToString()}");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["ExpiryInDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}