using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Tạo SSL Handler cho development
var handler = new HttpClientHandler();
if (builder.Environment.IsDevelopment())
{
    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
}

// Cấu hình HttpClient với port chính xác
builder.Services.AddHttpClient("AuthAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5085/"); // Port của AuthAPI (không dùng HTTPS)
}).ConfigurePrimaryHttpMessageHandler(() => handler);

// Các service khác nếu cần
builder.Services.AddHttpClient("ProductAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5058/");
}).ConfigurePrimaryHttpMessageHandler(() => handler);

builder.Services.AddHttpClient("OrderAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5198/");
}).ConfigurePrimaryHttpMessageHandler(() => handler);

builder.Services.AddHttpClient("ReportAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5230/");
}).ConfigurePrimaryHttpMessageHandler(() => handler);

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseSession();

app.MapRazorPages();

app.Run();