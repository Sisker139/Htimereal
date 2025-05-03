using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
