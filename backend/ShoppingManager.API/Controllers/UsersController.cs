using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingManager.API.DTOs;
using ShoppingManager.API.Services;

namespace ShoppingManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
                return NotFound(new { message = "User not found" });
            
            return Ok(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _userService.CreateUserAsync(registerDto);
            
            if (user == null)
                return BadRequest(new { message = "User with this email already exists" });
            
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            
            if (user == null)
                return NotFound(new { message = "User not found" });
            
            return Ok(user);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            
            if (!result)
                return NotFound(new { message = "User not found" });
            
            return Ok(new { message = "User deleted successfully" });
        }
        
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var result = await _userService.ToggleUserStatusAsync(id);
            
            if (!result)
                return NotFound(new { message = "User not found" });
            
            return Ok(new { message = "User status updated successfully" });
        }
        
        [HttpGet("{id}/login-history")]
        public async Task<IActionResult> GetUserLoginHistory(int id)
        {
            var history = await _userService.GetUserLoginHistoryAsync(id);
            return Ok(history);
        }
    }
}