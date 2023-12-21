using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Areas.Manage.ViewModels;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Models;
using Microsoft.AspNetCore.Authorization;


namespace WebApplication2.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
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
            var Products = await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Size)
                .Include(x => x.Color)
                .Include(x => x.Material)
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted).ToListAsync();
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
                string fileName = createProductVm.MainImage.SaveFile(_env.WebRootPath, "Upload");
                Image image = new Image
                {
                    ImgUrl = fileName,
                    IsMain = true
                };
                product.Images.Add(image);
            }

            foreach (var item in createProductVm.AdditionalImages)
            {
                string fileName = item.SaveFile(_env.WebRootPath, "Upload");
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

        public async Task<IActionResult> UpdateProduct(int id)
        {
			Product product = await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Size)
                .Include(x => x.Color)
                .Include(x => x.Material)
                .FirstOrDefaultAsync(x => x.Id == id);

			if (product == null)
            {
				return NotFound();
			}

			ViewBag.Category = await _context.Categories.ToListAsync();
			ViewBag.Size = await _context.Sizes.ToListAsync();
			ViewBag.Material = await _context.Materials.ToListAsync();
			ViewBag.Color = await _context.Colors.ToListAsync();

			UpdateProductVm updateProductVm = new UpdateProductVm
            {
				Name = product.Name,
				Price = product.Price,
				Description = product.Description,
				CategoryId = product.CategoryId,
				Sizes = product.Size,
                Colors = product.Color,
                Materials = product.Material,
                MaterialIds = product.Material.Select(x => x.Id).ToList(),
                ColorIds = product.Color.Select(x => x.Id).ToList(),
                SizeIds = product.Size.Select(x => x.Id).ToList(),
                Images = new List<ProductImageVm>(),
                Category = product.Category
			};

            foreach (var item in product.Images)
            {
                ProductImageVm productImageVm = new ProductImageVm
                {
                    Id = item.Id,
                    ImgUrl = item.ImgUrl,
                    IsMain = item.IsMain
                };

                updateProductVm.Images.Add(productImageVm);
            }
            
            

			return View(updateProductVm);
		}

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductVm updateProductVm)
        {
            Product product = await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Size)
                .Include(x => x.Color)
                .Include(x => x.Material)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = updateProductVm.Name;
            product.Price = updateProductVm.Price;
            product.Description = updateProductVm.Description;
            product.CategoryId = updateProductVm.CategoryId;
            product.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == updateProductVm.CategoryId);

            product.Size.RemoveAll(x => !updateProductVm.SizeIds.Contains(x.Id));
            product.Color.RemoveAll(x => !updateProductVm.ColorIds.Contains(x.Id));
            product.Material.RemoveAll(x => !updateProductVm.MaterialIds.Contains(x.Id));

            foreach (var item in updateProductVm.SizeIds)
            {
                if (!product.Size.Any(x => x.Id == item))
                {
                    product.Size.Add(await _context.Sizes.FirstOrDefaultAsync(x => x.Id == item));
                }
            }

            foreach (var item in updateProductVm.ColorIds)
            {
                if (!product.Color.Any(x => x.Id == item))
                {
                    product.Color.Add(await _context.Colors.FirstOrDefaultAsync(x => x.Id == item));
                }
            }

            foreach (var item in updateProductVm.MaterialIds)
            {
                if (!product.Material.Any(x => x.Id == item))
                {
                    product.Material.Add(await _context.Materials.FirstOrDefaultAsync(x => x.Id == item));
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Category = await _context.Categories.ToListAsync();
                ViewBag.Size = await _context.Sizes.ToListAsync();
                ViewBag.Material = await _context.Materials.ToListAsync();
                ViewBag.Color = await _context.Colors.ToListAsync();

                updateProductVm.Images = new List<ProductImageVm>();

                foreach (var item in product.Images)
                {
                    ProductImageVm productImageVm = new ProductImageVm
                    {
                        Id = item.Id,
                        ImgUrl = item.ImgUrl,
                        IsMain = item.IsMain
                    };

                    updateProductVm.Images.Add(productImageVm);
                }

                return View(updateProductVm);
            }

            if (updateProductVm.KeepImages != null)
            {
                List<Image> removedImages = product.Images.Where(x => !updateProductVm.KeepImages.Contains(x.Id) && x.IsMain == false).ToList();

                foreach (var item in removedImages)
                {
                    item.ImgUrl.DeleteFile(_env.WebRootPath, "Upload");
                    product.Images.Remove(item);
                }
            }
            else
            {
                product.Images.RemoveAll(x => x.IsMain == false);
            }

            if (updateProductVm.MainImage != null)
            {
                if (!updateProductVm.MainImage.IsImage())
                {
                    ModelState.AddModelError("MainImage", "Please select image type");
                    ViewBag.Category = await _context.Categories.ToListAsync();
                    ViewBag.Size = await _context.Sizes.ToListAsync();
                    ViewBag.Material = await _context.Materials.ToListAsync();
                    ViewBag.Color = await _context.Colors.ToListAsync();

                    return View(updateProductVm);
                }

                if (!updateProductVm.MainImage.IsSizeAllowed(1024 * 2))
                {
                    ModelState.AddModelError("MainImage", "Please select image size less than 2mb");
                    ViewBag.Category = await _context.Categories.ToListAsync();
                    ViewBag.Size = await _context.Sizes.ToListAsync();
                    ViewBag.Material = await _context.Materials.ToListAsync();
                    ViewBag.Color = await _context.Colors.ToListAsync();

                    return View(updateProductVm);
                }

                string fileName = updateProductVm.MainImage.SaveFile(_env.WebRootPath, "Upload");
                Image image = new Image
                {
                    ImgUrl = fileName,
                    IsMain = true
                };

                product.Images.RemoveAll(x => x.IsMain == true);
                product.Images.Add(image);
            }

            if (updateProductVm.AdditionalImages != null)
            {
                foreach (var item in updateProductVm.AdditionalImages)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("AdditionalImages", "Please select image type");
                        ViewBag.Category = await _context.Categories.ToListAsync();
                        ViewBag.Size = await _context.Sizes.ToListAsync();
                        ViewBag.Material = await _context.Materials.ToListAsync();
                        ViewBag.Color = await _context.Colors.ToListAsync();

                        return View(updateProductVm);
                    }

                    if (!item.IsSizeAllowed(1024 * 2))
                    {
                        ModelState.AddModelError("AdditionalImages", "Please select image size less than 2mb");
                        ViewBag.Category = await _context.Categories.ToListAsync();
                        ViewBag.Size = await _context.Sizes.ToListAsync();
                        ViewBag.Material = await _context.Materials.ToListAsync();
                        ViewBag.Color = await _context.Colors.ToListAsync();

                        return View(updateProductVm);
                    }

                    string fileName = item.SaveFile(_env.WebRootPath, "Upload");
                    Image image = new Image
                    {
                        ImgUrl = fileName,
                        IsMain = false
                    };
                    product.Images.Add(image);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}