using Microsoft.AspNetCore.Mvc;

namespace Htime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanlyDHController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
