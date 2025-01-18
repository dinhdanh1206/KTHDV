using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OrderManagement.Services
{
    public class OrderManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderManagementService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Lấy tất cả các đơn hàng
        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        // Lấy thông tin chi tiết một đơn hàng theo ID
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        // Tạo đơn hàng mới
        public async Task AddOrder(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        // Cập nhật trạng thái đơn hàng
        public async Task UpdateOrder(int id, Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(id);

            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            existingOrder.Status = order.Status;
            existingOrder.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Xóa một đơn hàng
        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        // Nếu bạn muốn kiểm tra tính hợp lệ của token trong dịch vụ, đảm bảo kiểm tra đầy đủ các yêu cầu bảo mật
        private bool IsValidToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtSettings = _configuration.GetSection("Jwt");
                var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                // Thực hiện xác thực đầy đủ
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                handler.ValidateToken(token, validationParameters, out var validatedToken);

                var claim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub");
                return claim != null;
            }
            catch
            {
                // Nếu token không hợp lệ, trả về false hoặc có thể ném ngoại lệ tùy thuộc vào tình huống
                return false;
            }
        }
    }
}
