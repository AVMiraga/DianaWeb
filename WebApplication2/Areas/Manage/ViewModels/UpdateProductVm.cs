using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.ViewModels
{
	public class UpdateProductVm
	{
		//==============================================================================
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public bool IsDeleted { get; set; }

		//==============================================================================
		public IFormFile MainImage { get; set; }
		public List<IFormFile> AdditionalImages { get; set; }
		public List<int> KeepImages { get; set; }
		public List<ProductImageVm> Images { get; set; }

		//==============================================================================
		public List<int> SizeIds { get; set; }
		public List<Size> Sizes { get; set; }

		//==============================================================================
		public List<int> ColorIds { get; set; }
		public List<Color> Colors { get; set; }

		//==============================================================================
		public List<int> MaterialIds { get; set; }
		public List<Material> Materials { get; set; }

		//==============================================================================
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}

	public class ProductImageVm
	{
		public int Id { get; set; }
		public string ImgUrl { get; set; }
		public bool IsMain { get; set; }
	}
}
