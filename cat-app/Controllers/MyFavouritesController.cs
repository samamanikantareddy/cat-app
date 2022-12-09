using cat_app.Data;
using cat_app.Models;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Controllers
{
    public class MyFavouritesController : Controller
    {
        private readonly CatDbContext _db;
        private List<Cat> FavouriteCats;

        public MyFavouritesController(CatDbContext db)
        {
            _db = db;
        }

        private bool FetchFavouriteCats()
        {
            var user = GetAuthenticatedUser();
            if (user != default(ApplicationUser))
                try
                {
                    FavouriteCats = (from c in _db.FavouriteCats
                                     where string.Equals(c.Fan.UserName, user!.UserName)
                                     select c).ToList();
                    TempData["Error"] = false;
                    return true;
                }
                catch (Exception e)
                {
                    TempData["Message"] = e.Message;
                    TempData["Error"] = true;
                    return false;
                }
            else
            {
                TempData["Message"] = "User not Found!";
                TempData["Error"] = true;
                return false;
            }
        }

        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }
            var success = FetchFavouriteCats();
            if (success)
                return View(FavouriteCats);
            return View();
        }

        public IActionResult RemoveFromFavourites(string id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }
            var cat = (from c in _db.FavouriteCats
                       where string.Equals(c.id, id)
                       select c).FirstOrDefault();
            if (cat != default(Cat))
            {
                _db.FavouriteCats.Remove(cat);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
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
