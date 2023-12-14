using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
