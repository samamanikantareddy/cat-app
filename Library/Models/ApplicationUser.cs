using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Cat>? FavouriteCats { get; set; }
    }
}
