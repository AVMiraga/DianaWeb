using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Product = _context.Products.Where(p => !p.IsDeleted).ToList();
            ViewBag.Category = _context.Categories;

            return View(Product);
        }
    }
}
