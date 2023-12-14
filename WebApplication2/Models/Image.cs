namespace WebApplication2.Models
{
    public class Image : BaseEntity
    {
        public string ImgUrl { get; set; }
        public int ProductId { get; set; }
        public bool IsMain { get; set; }
        public Product? Product { get; set; }
    }
}
