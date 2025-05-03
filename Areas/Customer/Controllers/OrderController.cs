using Htime.Data;
using Htime.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Htime.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDBContext _context;

        public OrderController(ApplicationDBContext context)
        {
            _context = context;
        }

        private int? GetUserId()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId(); // lấy từ cookie hoặc User.Claims
            if (userId == null) return RedirectToAction("Login", "Account");


            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            var result = orders.Select(o => new OrderViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                ShippingAddress = o.ShippingAddress,
                Total = o.Total,
                Status = o.Status,
                Items = o.OrderDetails
            }).ToList();

            return View(result);
        }

    }
}
