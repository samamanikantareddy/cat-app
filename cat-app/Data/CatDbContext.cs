using cat_app.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Data
{
    public class CatDbContext: IdentityDbContext<ApplicationUser>
    {
        public CatDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Cat> FavouriteCats { get; set; }
    }
}
