using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Areas.Manage.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVm create)
        {
            if(!ModelState.IsValid) return View(create);

            Slider slider = new Slider
            {
                Title = create.Title,
                Subtitle = create.Subtitle,
                IsDeleted = false,
                ImageUrl = create.Image.SaveFile(_env.WebRootPath, "Upload")
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider == null) return NotFound();

            UpdateSliderVm update = new UpdateSliderVm
            {
                Id = slider.Id,
                Title = slider.Title,
                Subtitle = slider.Subtitle,
                ImageUrl = slider.ImageUrl
            };

            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSliderVm update)
        {
            if(!ModelState.IsValid) return View(update);

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == update.Id);

            if (slider == null) return NotFound();

            slider.Title = update.Title;
            slider.Subtitle = update.Subtitle;

            if(update.Image != null)
            {
                slider.ImageUrl = update.Image.SaveFile(_env.WebRootPath, "Upload");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider == null) return NotFound();

            slider.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
