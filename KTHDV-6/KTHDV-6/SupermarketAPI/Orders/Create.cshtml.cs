using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupermarketAPI.Models;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Order Order { get; set; } = new Order
        {
            OrderItems = new List<OrderItem> { new OrderItem() }
        };

        public SelectList ProductList { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("ProductAPI");
            var products = await client.GetFromJsonAsync<List<Product>>("api/product");

            ProductList = new SelectList(products, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var client = _clientFactory.CreateClient("OrderAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            Order.Status = "pending";
            Order.CreatedAt = DateTime.Now;
            Order.UpdatedAt = DateTime.Now;

            var response = await client.PostAsJsonAsync("api/order", Order);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Error creating order");
            await OnGetAsync();
            return Page();
        }
    }
}