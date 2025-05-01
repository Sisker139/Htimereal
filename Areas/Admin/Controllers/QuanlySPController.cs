using Htime.Data;
using Htime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Htime.Areas.Admin.Controllers
{
    public class QuanlySPController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuanlySPController(ApplicationDBContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                Console.WriteLine("Product ID sau khi lưu: " + product.Id);
                return Json(new { success = true, product = product }); 
            }
            return Json(new { success = false, message = "Có lỗi xảy ra!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Brand = product.Brand;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Description = product.Description;
            existingProduct.IsActive = product.IsActive;

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", existingProduct.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                existingProduct.ImageUrl = product.ImageUrl;
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Cập nhật thành công." });
        }



        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                
                var filePath = Path.Combine(uploadsFolder, image.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                var imageUrl = image.FileName; // Đường dẫn lưu trong DB
                return Json(new { success = true, imageUrl });
            }
            return Json(new { success = false, message = "Không thể tải ảnh!" });
        }


        [HttpPost]
        public JsonResult UpdateStatus([FromBody] Product model)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);
                if (product == null)
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

                product.IsActive = model.IsActive;
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Json(products);
        }

    }
}
