using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("OrderAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            Orders = await client.GetFromJsonAsync<List<Order>>("api/order") ?? new List<Order>();
        }
    }
}