using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
    public class DashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
