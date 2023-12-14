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
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var colors = await _context.Colors.ToListAsync();

            return View(colors);
        }

        public IActionResult CreateColor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Color color = new Color
            {
                Name = propertyVm.Name
            };

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateColor(int id)
        {
            Color color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            CreatePropertyVm propertyVm = new CreatePropertyVm
            {
                Id = color.Id,
                Name = color.Name
            };

            return View(propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateColor(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Color color = await _context.Colors.FindAsync(propertyVm.Id);

            if (color == null)
            {
                return NotFound();
            }

            color.Name = propertyVm.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteColor(int id)
        {
            Color color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
