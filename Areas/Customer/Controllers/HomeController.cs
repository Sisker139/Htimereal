using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
