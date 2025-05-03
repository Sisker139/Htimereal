using Htime.Data;
using Htime.Models;
using Htime.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace Htime.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CartController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 10;
            
            var totalItems = _context.Carts.Count();
            var carts = _context.Carts.OrderBy(c => c.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var viewModel = new CartViewModel
            {
                Carts = carts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult GetQuantity([FromBody] Cart model)
        {
            try
            {
                var cart = _context.Carts.FirstOrDefault(c => c.Id == model.Id);
                if (cart == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }
                return Json(new { success = true, quantity = cart.Quantity, price = cart.Price });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteCart([FromBody] Cart cart)
        {
            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == cart.Id);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
            }

            _context.Carts.Remove(cartItem);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var allCartItems = _context.Carts.ToList();
            _context.Carts.RemoveRange(allCartItems);
            _context.SaveChanges();
            return Json(new { success = true });
        }


    }
}
