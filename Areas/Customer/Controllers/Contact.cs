using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Customer.Controllers
{
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
