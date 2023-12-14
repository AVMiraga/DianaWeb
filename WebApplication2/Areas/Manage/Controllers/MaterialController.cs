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
    public class MaterialController : Controller
    {
        private readonly AppDbContext _context;
        public MaterialController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var materials = await _context.Materials.ToListAsync();

            return View(materials);
        }

        public IActionResult CreateMaterial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Material material = new Material
            {
                Name = propertyVm.Name
            };

            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateMaterial(int id)
        {
            Material material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            CreatePropertyVm propertyVm = new CreatePropertyVm
            {
                Id = material.Id,
                Name = material.Name
            };

            return View(propertyVm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMaterial(CreatePropertyVm propertyVm)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyVm);
            }

            Material material = await _context.Materials.FindAsync(propertyVm.Id);

            if (material == null)
            {
                return NotFound();
            }

            material.Name = propertyVm.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMaterial(int id)
        {
            Material material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
