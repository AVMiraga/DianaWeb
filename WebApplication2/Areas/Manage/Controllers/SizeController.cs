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
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sizes = await _context.Sizes.ToListAsync();

            return View(sizes);
        }

        public IActionResult CreateSize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Size size = new Size
            {
                Name = propertyVm.Name
            };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateSize(int id)
        {
            Size size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            CreatePropertyVm propertyVm = new CreatePropertyVm
            {
                Id = size.Id,
                Name = size.Name
            };

            return View(propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSize(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Size size = await _context.Sizes.FindAsync(propertyVm.Id);

            if (size == null)
            {
                return NotFound();
            }

            size.Name = propertyVm.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSize(int id)
        {
            Size size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
