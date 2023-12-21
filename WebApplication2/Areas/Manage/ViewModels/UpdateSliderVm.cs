namespace WebApplication2.Areas.Manage.ViewModels
{
    public class UpdateSliderVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
