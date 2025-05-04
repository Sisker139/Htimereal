using Htime.Data;
using Htime.Models;
using Htime.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        private int? GetUserId()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return user?.Id;
        }


        public IActionResult Index(int page = 1)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            int pageSize = 10;
            var userCarts = _context.Carts.Where(c => c.UserId == userId);

            var totalItems = userCarts.Count();
            var carts = userCarts.OrderBy(c => c.Id)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

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

        public class AddToCartRequest
        {
            public int ProductId { get; set; }
            public int SelectedQuantity { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Json(new { success = false, requireLogin = true, message = "Vui lòng đăng nhập để thêm vào giỏ hàng." });
            }

            //if (userId == null) return Unauthorized();

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null) return NotFound();
            var existingCart = await _context.Carts
                            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == request.ProductId);
            if (product.StockQuantity == 0)
            {
                return Json(new { success = false, message = "Hết hàng" });
            }
            else if (request.SelectedQuantity > product.StockQuantity)
                return Json(new { success = false, message = "Số lượng vượt quá tồn kho!" });

            else if (existingCart != null)
            {
                return Json(new { success = false, message = "Sản phẩm đã có trong giỏ hàng." });
            }

            

            

            var cartItem = new Cart
            {
                UserId = userId.Value,
                ProductId = product.Id,
                Name = product.Name,
                Image = product.ImageUrl,
                Price = product.Price,
                Quantity = product.StockQuantity,
                SelectedQuantity = request.SelectedQuantity
            };

            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();

            return Json(new { success = true, selectedquantity = request.SelectedQuantity, message = "Đã thêm vào giỏ hàng!" });
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

        public class CheckoutRequest
        {
            public string ShippingAddress { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();


            if (string.IsNullOrWhiteSpace(request.ShippingAddress))
                return Json(new { success = false, message = "Vui lòng nhập địa chỉ giao hàng." });

            var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            if (!cartItems.Any())
                return Json(new { success = false, message = "Giỏ hàng trống." });

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Total = cartItems.Sum(c => c.Price * c.SelectedQuantity),
                ShippingAddress = request.ShippingAddress,
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductId = c.ProductId,
                    Name = c.Name,
                    Image = c.Image,
                    Price = c.Price,
                    Quantity = c.SelectedQuantity
                }).ToList()
            };

            foreach (var item in order.OrderDetails)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                if (product.StockQuantity < item.Quantity)
                {
                    return BadRequest($"Not enough stock for product: {product.Name}");
                }

                product.StockQuantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }




    }
}
