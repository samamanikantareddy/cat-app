using cat_app.DataCache;
using Library.DataAccess;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace cat_app.Controllers
{
    public class CatsController : Controller
    {
        private readonly IProvider<Cat> _provider;

        private readonly CatStore store;
        
        private List<Cat>? Cats;

        private bool isSuccess = true;

        public CatsController(CatStore catStore, IProvider<Cat> provider)
        {
            _provider = provider;
            store = catStore;
            try
            {
                Cats = store.GetCats();
                isSuccess = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                    var exists = user.FavouriteCats.Exists(_cat => string.Equals(_cat.id, cat.id));
                    if (!exists)
                    {
                        cat.Fan = user;
                        user.FavouriteCats.Add(cat);
                        _provider.AddToFavourites(cat);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        private ApplicationUser? GetAuthenticatedUser()
        {
            var username = User.Identity!.Name;
            return _provider.GetUser(username!);
        }
    }
}
