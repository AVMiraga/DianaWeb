using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication2.Areas.Manage.ViewModel;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryVm);
            }

            Category category = new Category
            {
                Name = categoryVm.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
