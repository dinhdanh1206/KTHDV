using Microsoft.EntityFrameworkCore;
using ReportingAPI.Data;
using ReportingAPI.Models;

namespace ReportingAPI.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Product Reports
        public async Task<List<ProductReport>> GetAllProductReports()
        {
            return await _context.ProductReports.ToListAsync();
        }

        public async Task<ProductReport?> GetProductReportById(int id)
        {
            return await _context.ProductReports.FindAsync(id);
        }

        public async Task<ProductReport> CreateProductReport(ProductReport report)
        {
            // Tính toán lợi nhuận
            report.Profit = report.Revenue - report.Cost;

            _context.ProductReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task DeleteProductReport(int id)
        {
            var report = await _context.ProductReports.FindAsync(id);
            if (report != null)
            {
                _context.ProductReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }

        // Order Reports
        public async Task<List<OrderReport>> GetAllOrderReports()
        {
            return await _context.OrderReports.ToListAsync();
        }

        public async Task<OrderReport?> GetOrderReportById(int id)
        {
            return await _context.OrderReports.FindAsync(id);
        }

        public async Task<OrderReport> CreateOrderReport(OrderReport report)
        {
            // Tính toán tổng lợi nhuận
            report.TotalProfit = report.TotalRevenue - report.TotalCost;

            _context.OrderReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task DeleteOrderReport(int id)
        {
            var report = await _context.OrderReports.FindAsync(id);
            if (report != null)
            {
                _context.OrderReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}