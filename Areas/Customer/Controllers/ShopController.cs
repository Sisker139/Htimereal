using Htime.Data;
using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDBContext db;

        public ShopController(ApplicationDBContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var products = db.Products.Where(p => p.IsActive).ToList();
            return View(products);
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
