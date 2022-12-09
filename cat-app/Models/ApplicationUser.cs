using Microsoft.AspNetCore.Identity;

namespace cat_app.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Cat> FavouriteCats { get; set; }
    }
}
