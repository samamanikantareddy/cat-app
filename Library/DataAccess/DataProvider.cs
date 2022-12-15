using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess
{
    public class DataProvider : IProvider<Cat>
    {
        private readonly CatDbContext _db;
        public DataProvider(CatDbContext db)
        {
            _db = db;
        }

        public void Delete(string id)
        {
            _db.FavouriteCats!.Remove(GetById(id)!);
            _db.SaveChanges();
        }

        public List<Cat> GetAll()
        {
            return _db.FavouriteCats!.ToList();
        }

        public List<Cat> GetByBreed(string breedId)
        {
            return _db.FavouriteCats!.Where(cat => cat.breeds!.Exists(breed => string.Equals(breed.id, breedId))).ToList();
        }

        public Cat? GetById(string id)
        {
            return (from c in _db.FavouriteCats
                    where string.Equals(c.id, id)
                    select c).FirstOrDefault();
        }

        public List<Cat> GetFavouritesByUser(string username)
        {

            return (from c in _db.FavouriteCats
                    where string.Equals(c.Fan!.UserName, username)
                    select c).ToList();
        }

        public void AddToFavourites(Cat cat)
        {
            _db.FavouriteCats!.Add(cat);
            _db.SaveChanges();
        }

        public ApplicationUser? GetUser(string username)
        {
            return (from u in _db.Users
                    where string.Equals(username, u.UserName)
                    select u)
                .Include(_user => _user.FavouriteCats!)
                .FirstOrDefault();
        }
    }
}
