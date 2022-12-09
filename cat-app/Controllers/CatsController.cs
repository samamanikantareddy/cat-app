using cat_app.Data;
using cat_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Controllers
{
    public class CatsController : Controller
    {
        private readonly CatDbContext _db;

        public CatsController(CatDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        private ApplicationUser? GetAuthenticatedUser()
        {
            var username = User.Identity!.Name;
            return (from u in _db.Users
                    where string.Equals(username, u.UserName)
                    select u)
                .Include(_user => _user.FavouriteCats!)
                .FirstOrDefault();
        }
    }
}
