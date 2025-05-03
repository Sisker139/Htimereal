using Htime.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? C?u hình xác th?c b?ng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // ???ng d?n ??n trang ??ng nh?p n?u ch?a ??ng nh?p
        options.LoginPath = "/Customer/DangNhap/Login";

        // ???ng d?n n?u b? t? ch?i truy c?p
        options.AccessDeniedPath = "/Customer/DangNhap/AccessDenied";

        // (Tu? ch?n) Redirect sau khi ??ng nh?p thành công
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ? Thêm middleware xác th?c và phân quy?n
app.UseAuthentication();
app.UseAuthorization();

// ? Routing m?c ??nh có Area
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
