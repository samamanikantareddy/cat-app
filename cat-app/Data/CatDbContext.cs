using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Data
{
    public class CatDbContext: IdentityDbContext
    {
        public CatDbContext(DbContextOptions options): base(options)
        {
        }
    }
}
