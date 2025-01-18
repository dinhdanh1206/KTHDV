using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupermarketAPI.Models;
using System.Net.Http.Json;

namespace SupermarketAPI.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public EditModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Order Order { get; set; }
        public SelectList ProductList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var orderClient = _clientFactory.CreateClient("OrderAPI");
            var productClient = _clientFactory.CreateClient("ProductAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                orderClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                productClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            Order = await orderClient.GetFromJsonAsync<Order>($"api/order/{id}");
            if (Order == null)
            {
                return NotFound();
            }

            var products = await productClient.GetFromJsonAsync<List<Product>>("api/product");
            ProductList = new SelectList(products, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var productClient = _clientFactory.CreateClient("ProductAPI");
                var products = await productClient.GetFromJsonAsync<List<Product>>("api/product");
                ProductList = new SelectList(products, "Id", "Name");
                return Page();
            }

            var client = _clientFactory.CreateClient("OrderAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            Order.UpdatedAt = DateTime.Now;

            var response = await client.PutAsJsonAsync($"api/order/{Order.Id}", Order);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Error updating order");
            return Page();
        }
    }
}