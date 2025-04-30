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
                return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
            }
            return Json(new { success = false, message = "Có lỗi xảy ra!" });
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

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Json(products);
        }

    }
}
