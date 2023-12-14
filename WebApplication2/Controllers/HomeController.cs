using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var Product = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Category = _context.Categories;

            return View(Product);
        }
    }
}
