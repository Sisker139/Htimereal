using Htime.Data;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace Htime.Areas.Customer.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDBContext db;

        public ShopController(ApplicationDBContext context)
        {
            db = context;
        }
        public IActionResult Index(string? brand, int page = 1)
        {
            int pageSize = 9;
            var productsQuery = db.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(brand))
            {
                productsQuery = productsQuery.Where(p => p.Brand == brand);
            }

            var pagedProducts = productsQuery.OrderBy(p => p.Id).ToPagedList(page, pageSize);

            var brandCounts = db.Products
                .Where(p => p.IsActive)
                .GroupBy(p => p.Brand)
                .Select(g => new { Brand = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.BrandCounts = brandCounts;
            ViewBag.SelectedBrand = brand;

            return View(pagedProducts);
        }




        public IActionResult Detail(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }






    }

}
