using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models;

namespace WebApplication2.Areas.Manage.ViewModels
{
    public class CreateProductVm
    {
        //==============================================================================
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        //==============================================================================
        public IFormFile MainImage { get; set; }
        public List<IFormFile> AdditionalImages { get; set; }

        //==============================================================================
        public List<int> SizeIds { get; set; }

        //==============================================================================
        public List<int> ColorIds { get; set; }

        //==============================================================================
        public List<int> MaterialIds { get; set; }

        //==============================================================================
        public int CategoryId { get; set; }
    }
}