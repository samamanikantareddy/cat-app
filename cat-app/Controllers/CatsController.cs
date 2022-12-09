using cat_app.Data;
using cat_app.DataCache;
using cat_app.Models;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Controllers
{
    public class CatsController : Controller
    {
        private readonly CatDbContext _db;

        private readonly CatStore store;
        
        private List<Cat>? Cats;

        private bool isSuccess = true;

        public CatsController(CatDbContext db, CatStore catStore)
        {
            _db = db;
            store = catStore;
            try
            {
                Cats = store.GetCats();
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
        }

        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }
            TempData["Error"] = !isSuccess;
            if (isSuccess)
                return View(Cats);
            TempData["Message"] = "Service unavailable! check your internet connection";
            return View();
        }

        public IActionResult AddToFavourites(string id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }
            var user = GetAuthenticatedUser();
            if (user != default(ApplicationUser))
            {
                var cat = Cats!.FirstOrDefault(_cat => string.Equals(_cat.id, id));
                if (cat != default(Cat))
                {
                    cat.Fan = user;
                    //from c in user.FavouriteCats;
                    var exists = user.FavouriteCats.Exists(_cat => string.Equals(_cat.id, cat.id));
                    if (!exists)
                    {
                        cat.Fan = user;
                        user.FavouriteCats.Add(cat);
                        _db.SaveChanges();
                    }
                }
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
