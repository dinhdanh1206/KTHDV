using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _logger = logger;
        }

        // API Đăng Ký Người Dùng
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username và password không được để trống.");
            }

            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
            {
                return BadRequest("Username đã tồn tại.");
            }

            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            var user = new User
            {
                UserName = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                CreatedAt = DateTime.Now,
                Token = string.Empty
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("Đăng ký thành công. Vui lòng đăng nhập.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // API Đăng Nhập và Sinh JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName);

                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(null, user.Password, request.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid username or password");
                }

                // Tạo token
                var token = GenerateJwtToken(user);

                // Cập nhật token trong database
                user.Token = token;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Kiểm Tra Người Dùng
        private User AuthenticateUser(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            return user;
        }

        // Sinh JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
