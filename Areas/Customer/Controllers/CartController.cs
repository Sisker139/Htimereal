using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
