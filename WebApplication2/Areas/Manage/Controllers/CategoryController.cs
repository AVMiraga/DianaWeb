using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication2.Areas.Manage.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Category category = new Category
            {
                Name = propertyVm.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            CreatePropertyVm propertyVm = new CreatePropertyVm
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Category category = await _context.Categories.FindAsync(propertyVm.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = propertyVm.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
