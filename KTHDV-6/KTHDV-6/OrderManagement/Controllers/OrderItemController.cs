using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrderItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/order_items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
    {
        try
        {
            var orderItems = await _context.OrderItems.ToListAsync();
            return Ok(orderItems);
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    // GET: api/order_items/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);

        if (orderItem == null)
        {
            return NotFound();
        }

        return Ok(orderItem); // Trả về thông tin đơn hàng với mã 200 OK
    }

    // POST: api/order_items
    [HttpPost]
    public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
    {
        if (orderItem == null)
        {
            return BadRequest("Order item data is null.");
        }

        orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        try
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
        }
        catch
        {
            return StatusCode(500, "Error while saving the order item.");
        }
    }

    // PUT: api/order_items/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
    {
        if (id != orderItem.Id)
        {
            return BadRequest("Order item ID mismatch.");
        }

        var existingOrderItem = await _context.OrderItems.FindAsync(id);

        if (existingOrderItem == null)
        {
            return NotFound();
        }

        existingOrderItem.Quantity = orderItem.Quantity;
        existingOrderItem.UnitPrice = orderItem.UnitPrice;
        existingOrderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        try
        {
            _context.Entry(existingOrderItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Error while updating the order item.");
        }
    }

    // DELETE: api/order_items/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            return NotFound();
        }

        try
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Error while deleting the order item.");
        }
    }
}
