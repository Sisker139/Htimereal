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


        public IActionResult Detail()
        {
            return View();
        }
    }
}
