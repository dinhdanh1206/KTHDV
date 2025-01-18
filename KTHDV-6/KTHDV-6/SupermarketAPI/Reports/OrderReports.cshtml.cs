using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketAPI.Models;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Reports
{
    public class OrderReportsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public OrderReportsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int DateRange { get; set; } = 30;

        [BindProperty(SupportsGet = true)]
        public string Status { get; set; } = "";

        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal ProfitMargin { get; set; }
        public List<OrderReportViewModel> OrderReports { get; set; } = new();
        public List<DailyStatViewModel> DailyStats { get; set; } = new();
        public int[] StatusDistribution { get; set; } = new int[3]; // [Pending, Completed, Cancelled]

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("ReportAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var reportData = await client.GetFromJsonAsync<ReportData>(
                $"api/report/orders?days={DateRange}&status={Status}");

            if (reportData != null)
            {
                TotalOrders = reportData.TotalOrders;
                TotalRevenue = reportData.TotalRevenue;
                AverageOrderValue = reportData.AverageOrderValue;
                ProfitMargin = reportData.ProfitMargin;
                OrderReports = reportData.Orders;
                DailyStats = reportData.DailyStats;
                StatusDistribution = reportData.StatusDistribution;
            }
        }
    }

    public class OrderReportViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ItemCount { get; set; }
        public decimal Profit { get; set; }
    }

    public class DailyStatViewModel
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ReportData
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal ProfitMargin { get; set; }
        public List<OrderReportViewModel> Orders { get; set; }
        public List<DailyStatViewModel> DailyStats { get; set; }
        public int[] StatusDistribution { get; set; }
    }
}