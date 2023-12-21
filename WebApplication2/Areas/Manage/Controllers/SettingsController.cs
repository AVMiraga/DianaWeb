using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return View(settings);
        }

        public async Task<IActionResult> Update()
        {
            Dictionary<string, string> settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Dictionary<string, string> settings, IFormFile? logo)
        {
            if (logo != null)
            {
                if (!logo.IsImage())
                {
                    ModelState.AddModelError("Logo", "You can upload only jpeg or png file");
                    return View();
                }

                if (!logo.IsSizeAllowed(2048))
                {
                    ModelState.AddModelError("Logo", "You can upload file only 5MB");
                    return View();
                }

                var fileName = logo.SaveFile(_env.WebRootPath, "Uploads");

                var setting = _context.Settings.FirstOrDefault(s => s.Key == "Logo");

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = "Logo",
                        Value = fileName
                    };

                    await _context.Settings.AddAsync(setting);
                }
                else
                {
                    setting.Value.DeleteFile(_env.WebRootPath, "Uploads");

                    setting.Value = fileName;
                }

                await _context.SaveChangesAsync();
            }

            foreach (var item in settings)
            {
                var setting = _context.Settings.FirstOrDefault(s => s.Key == item.Key);


                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = item.Key,
                        Value = item.Value ?? ""
                    };

                    await _context.Settings.AddAsync(setting);
                }
                else
                {
                    setting.Value = item.Value ?? "";
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
