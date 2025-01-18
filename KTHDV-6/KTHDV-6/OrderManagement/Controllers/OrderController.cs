using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Services;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bảo vệ các route bằng JWT
    public class OrderController : ControllerBase
    {
        private readonly OrderManagementService _service;

        public OrderController(OrderManagementService service)
        {
            _service = service;
        }

        // Lấy tất cả đơn hàng
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return Ok(await _service.GetAllOrders());
        }

        // Lấy thông tin đơn hàng theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _service.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // Tạo đơn hàng mới
        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            await _service.AddOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // Cập nhật trạng thái đơn hàng
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            var existingOrder = await _service.GetOrderById(id);
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            await _service.UpdateOrder(id, order);
            return NoContent();
        }

        // Xóa đơn hàng
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var existingOrder = await _service.GetOrderById(id);
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            await _service.DeleteOrder(id);
            return NoContent();
        }
    }
}
