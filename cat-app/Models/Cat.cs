namespace cat_app.Models
{
    public class Cat
    {
        public string id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
