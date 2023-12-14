namespace WebApplication2.Models
{
    public class Size : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
