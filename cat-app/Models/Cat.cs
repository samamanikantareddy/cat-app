using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cat_app.Models
{
    public class Cat
    {
        [Key, Column(Order = 0)]
        public string id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        [Key, Column(Order = 1)]
        public ApplicationUser Fan { get; set; }
    }
}
