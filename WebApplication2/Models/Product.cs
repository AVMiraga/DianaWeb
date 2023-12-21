using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public List<Image> Images { get; set; }

        public List<Size> Size { get; set; }
        [NotMapped]
        public List<int> SizeIds { get; set; }

        public List<Color> Color { get; set; }
        [NotMapped]
        public List<int> ColorIds { get; set; }

        public List<Material> Material { get; set; }
        [NotMapped]
        public List<int> MaterialIds { get; set; }

        public Category Category { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
    }
}
