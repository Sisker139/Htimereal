using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
