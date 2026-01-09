using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pitzam.Models;
using Pitzam.Server.Data;
using Pitzam.Server.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace Pitzam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PizzaStoreContext _context;

        public AuthController(PizzaStoreContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor." });
            }

            user.Password = HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Kayıt başarılı." });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
        {
            var hashedPassword = HashPassword(request.Password);
            var user = await _context.Users
                .Include(u => u.SavedAddresses)
                // .Include(u => u.SavedCards) // If mapped
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == hashedPassword);

            if (user == null)
            {
                return BadRequest(new { message = "E-posta veya şifre hatalı." });
            }

            // Map to DTO
            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                SavedAddresses = user.SavedAddresses?.Select(a => new CustomerDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Address = a.Address
                }).ToList() ?? new List<CustomerDto>(),
                // SavedCards manual mapping if they are present in JSON/DB but User.cs marked NotMapped in original
                // User.cs says SavedCards is NotMapped. So they won't be in DB. 
                // So we return empty list or what was in 'user' if it was somehow preserved? 
                // DB has no SavedCards. So empty.
                SavedCards = new List<SavedCardDto>()
            };

            return Ok(userDto);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(User updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Address = updatedUser.Address;
            
            // Note: Password update handled separately or check logic
            
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
             var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
             if (user == null) return NotFound("Kullanıcı bulunamadı.");
             
             var currentHashed = user.Password;
             var oldHashed = HashPassword(request.OldPassword);
             
             if (currentHashed != oldHashed) return BadRequest(new { message = "Mevcut şifre hatalı." });
             
             user.Password = HashPassword(request.NewPassword);
             await _context.SaveChangesAsync();
             
             return Ok(new { success = true });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public class LoginRequest { public string Email { get; set; } = ""; public string Password { get; set; } = ""; }
    public class ChangePasswordRequest { public string UserId { get; set; } = ""; public string OldPassword { get; set; } = ""; public string NewPassword { get; set; } = ""; }
}
