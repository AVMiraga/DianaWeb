namespace WebApplication2.Models
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
