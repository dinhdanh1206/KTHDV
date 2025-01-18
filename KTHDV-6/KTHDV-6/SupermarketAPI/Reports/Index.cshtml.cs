using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal AverageOrderValue { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("ReportAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var summary = await client.GetFromJsonAsync<DashboardSummary>("api/report/summary");
            if (summary != null)
            {
                TotalRevenue = summary.TotalRevenue;
                TotalOrders = summary.TotalOrders;
                TotalProducts = summary.TotalProducts;
                AverageOrderValue = summary.AverageOrderValue;
            }
        }

        private class DashboardSummary
        {
            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
            public int TotalProducts { get; set; }
            public decimal AverageOrderValue { get; set; }
        }
    }
}