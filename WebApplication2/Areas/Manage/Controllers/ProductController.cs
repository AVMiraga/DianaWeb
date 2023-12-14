using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Areas.Manage.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var Products = await _context.Products.ToListAsync();
            return View(Products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Category = await _context.Categories.ToListAsync();
            ViewBag.Size = await _context.Sizes.ToListAsync();
            ViewBag.Material = await _context.Materials.ToListAsync();
            ViewBag.Color = await _context.Colors.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductVm createProductVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Category = await _context.Categories.ToListAsync();
                ViewBag.Size = await _context.Sizes.ToListAsync();
                ViewBag.Material = await _context.Materials.ToListAsync();
                ViewBag.Color = await _context.Colors.ToListAsync();

                return View(createProductVm);
            }

            if (!createProductVm.MainImage.IsImage())
            {
                ModelState.AddModelError("MainImage", "Please select image type");
                ViewBag.Category = await _context.Categories.ToListAsync();
                ViewBag.Size = await _context.Sizes.ToListAsync();
                ViewBag.Material = await _context.Materials.ToListAsync();
                ViewBag.Color = await _context.Colors.ToListAsync();

                return View(createProductVm);
            }

            if (!createProductVm.MainImage.IsSizeAllowed(1024 * 2))
            {
                ModelState.AddModelError("MainImage", "Please select image size less than 2mb");
                ViewBag.Category = await _context.Categories.ToListAsync();
                ViewBag.Size = await _context.Sizes.ToListAsync();
                ViewBag.Material = await _context.Materials.ToListAsync();
                ViewBag.Color = await _context.Colors.ToListAsync();

                return View(createProductVm);
            }

            foreach (var item in createProductVm.AdditionalImages)
            {
                if (!item.IsImage())
                {
                    ModelState.AddModelError("AdditionalImages", "Please select image type");
                    ViewBag.Category = await _context.Categories.ToListAsync();
                    ViewBag.Size = await _context.Sizes.ToListAsync();
                    ViewBag.Material = await _context.Materials.ToListAsync();
                    ViewBag.Color = await _context.Colors.ToListAsync();

                    return View(createProductVm);
                }

                if (!item.IsSizeAllowed(1024 * 2))
                {
                    ModelState.AddModelError("AdditionalImages", "Please select image size less than 2mb");
                    ViewBag.Category = await _context.Categories.ToListAsync();
                    ViewBag.Size = await _context.Sizes.ToListAsync();
                    ViewBag.Material = await _context.Materials.ToListAsync();
                    ViewBag.Color = await _context.Colors.ToListAsync();

                    return View(createProductVm);
                }
            }

            Product product = new Product
            {
                Name = createProductVm.Name,
                Price = createProductVm.Price,
                Description = createProductVm.Description,
                IsDeleted = false,
                Images = new List<Image>(),
                Size = new List<Size>(),
                Color = new List<Color>(),
                Material = new List<Material>(),
                Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == createProductVm.CategoryId)
            };

            if (createProductVm.MainImage != null)
            {
                string fileName = createProductVm.MainImage.SaveFile(_env.WebRootPath, "img");
                Image image = new Image
                {
                    ImgUrl = fileName,
                    IsMain = true
                };
                product.Images.Add(image);
            }

            foreach (var item in createProductVm.AdditionalImages)
            {
                string fileName = item.SaveFile(_env.WebRootPath, "img");
                Image image = new Image
                {
                    ImgUrl = fileName,
                    IsMain = false
                };
                product.Images.Add(image);
            }

            foreach (var item in createProductVm.SizeIds)
            {
                product.Size.Add(await _context.Sizes.FirstOrDefaultAsync(x => x.Id == item));
            }

            foreach (var item in createProductVm.ColorIds)
            {
                product.Color.Add(await _context.Colors.FirstOrDefaultAsync(x => x.Id == item));
            }

            foreach (var item in createProductVm.MaterialIds)
            {
                product.Material.Add(await _context.Materials.FirstOrDefaultAsync(x => x.Id == item));
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}