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
                .OrderByDescending(o => o.OrderDate)
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

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.UserId != userId)
                return NotFound();

            if (order.Status != "Pending") // dùng enum/class nếu có
                return BadRequest("Chỉ có thể hủy đơn hàng đang chờ xác nhận.");

            foreach (var item in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                }
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
