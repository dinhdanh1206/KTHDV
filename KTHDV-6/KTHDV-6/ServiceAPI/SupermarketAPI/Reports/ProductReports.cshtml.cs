using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketAPI.Models;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Reports
{
    public class ProductReportsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductReportsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int DateRange { get; set; } = 30; // Default to 30 days

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "revenue"; // Default sort by revenue

        public List<ProductReportViewModel> ProductReports { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("ReportAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var reports = await client.GetFromJsonAsync<List<ProductReport>>(
                $"api/report/products?days={DateRange}&sortBy={SortBy}");

            if (reports != null)
            {
                ProductReports = reports.Select(r => new ProductReportViewModel
                {
                    ProductName = r.ProductName,
                    TotalSold = r.TotalSold,
                    Revenue = r.Revenue,
                    Cost = r.Cost,
                    Profit = r.Profit
                }).ToList();
            }
        }
    }

    public class ProductReportViewModel
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
    }
}