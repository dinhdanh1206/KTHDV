using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            ILogger<RegisterModel> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public RegisterViewModel RegisterData { get; set; }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Username is required")]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
            // Initialize if needed
            RegisterData = new RegisterViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _clientFactory.CreateClient("AuthAPI");

                var registerData = new
                {
                    UserName = RegisterData.UserName,
                    Email = RegisterData.Email,
                    Password = RegisterData.Password,
                    Role = "user"
                };

                _logger.LogInformation($"Attempting to register user: {RegisterData.UserName}");

                var content = new StringContent(
                    JsonSerializer.Serialize(registerData),
                    Encoding.UTF8,
                    "application/json"
                );

                // Gọi đến endpoint đăng ký của AuthAPI
                var response = await client.PostAsync("api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"User {RegisterData.UserName} registered successfully");
                    TempData["SuccessMessage"] = "Registration successful! Please login.";
                    return RedirectToPage("/Account/Login");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Registration failed: {errorContent}");
                ModelState.AddModelError(string.Empty, $"Registration failed: {errorContent}");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}