using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Cat
    {
        [Key, Column(Order = 0)]
        public string? id { get; set; }
        public string? url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        [Key, Column(Order = 1)]
        public ApplicationUser? Fan { get; set; }
        public List<Breed>? breeds { get; set; }
        public List<Category>? categories { get; set; }
    }
}
