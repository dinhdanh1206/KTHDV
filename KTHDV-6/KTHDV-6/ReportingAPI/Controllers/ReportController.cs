using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportingAPI.Models;
using ReportingAPI.Services;

namespace ReportingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<ProductReport>>> GetAllProductReports()
        {
            return Ok(await _service.GetAllProductReports());
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<ProductReport>> GetProductReport(int id)
        {
            var report = await _service.GetProductReportById(id);
            if (report == null)
                return NotFound();
            return Ok(report);
        }

        [HttpPost("products")]
        public async Task<ActionResult<ProductReport>> CreateProductReport(ProductReport report)
        {
            var newReport = await _service.CreateProductReport(report);
            return CreatedAtAction(nameof(GetProductReport), new { id = newReport.Id }, newReport);
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProductReport(int id)
        {
            await _service.DeleteProductReport(id);
            return NoContent();
        }

        [HttpGet("orders")]
        public async Task<ActionResult<List<OrderReport>>> GetAllOrderReports()
        {
            return Ok(await _service.GetAllOrderReports());
        }

        [HttpGet("orders/{id}")]
        public async Task<ActionResult<OrderReport>> GetOrderReport(int id)
        {
            var report = await _service.GetOrderReportById(id);
            if (report == null)
                return NotFound();
            return Ok(report);
        }

        [HttpPost("orders")]
        public async Task<ActionResult<OrderReport>> CreateOrderReport(OrderReport report)
        {
            var newReport = await _service.CreateOrderReport(report);
            return CreatedAtAction(nameof(GetOrderReport), new { id = newReport.Id }, newReport);
        }

        [HttpDelete("orders/{id}")]
        public async Task<IActionResult> DeleteOrderReport(int id)
        {
            await _service.DeleteOrderReport(id);
            return NoContent();
        }
    }
}