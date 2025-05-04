using Htime.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Htime.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                .ToListAsync();

            return View(orders); // TRUYỀN MODEL vào view
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            if (order.Status == "Confirmed")
                return BadRequest("Order already confirmed.");

            
            order.Status = "Confirmed";

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Order confirmed and stock updated." });
        }


        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            if (order.Status != "Pending")
                return BadRequest("Không thể hủy đơn đã xác nhận.");

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
