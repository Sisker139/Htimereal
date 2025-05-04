using Htime.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Htime.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanlyDHController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuanlyDHController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders); // TRUYỀN MODEL vào view
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromBody] int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng." });

            if (order.Status == "Confirmed")
                return BadRequest(new { message = "Đơn hàng đã được xác nhận." });

            order.Status = "Confirmed";
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xác nhận đơn hàng thành công." });
        }



        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            if (order.Status != "Pending")
                return BadRequest("Không thể hủy đơn đã xác nhận.");

            // Hoàn lại hàng vào kho
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
