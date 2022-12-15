using Library.DataAccess;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cat_app.Controllers
{
    public class MyFavouritesController : Controller
    {
        private List<Cat> FavouriteCats;
        private readonly IProvider<Cat> _provider;

        public MyFavouritesController(IProvider<Cat> provider)
        {
            _provider = provider;
        }

        private bool FetchFavouriteCats()
        {
            var user = GetAuthenticatedUser();
            if (user != default(ApplicationUser))
                try
                {
                    FavouriteCats = _provider.GetFavouritesByUser(user.UserName!);
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
            var cat = _provider.GetById(id);
            if (cat != default(Cat))
            {
                _provider.Delete(id);
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
