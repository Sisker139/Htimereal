using Htime.Data;
using Htime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace Htime.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class dangky : Controller
    {
        private readonly ApplicationDBContext db;

        public dangky (ApplicationDBContext Context)
        {
            db = Context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangKy(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var khachHang = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.MatKhau
                };

                db.Users.Add(khachHang);
                db.SaveChanges();

                // Tạo Claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, khachHang.Name),
            new Claim(ClaimTypes.Email, khachHang.Email)
        };

                var identity = new ClaimsIdentity(claims, "UserScheme");
                var principal = new ClaimsPrincipal(identity);

                // Đăng nhập
                await HttpContext.SignInAsync("Cookies", principal);

                // Chuyển hướng tới trang chính (hoặc trang chào mừng)
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

    }
}   
