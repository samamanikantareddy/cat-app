
using Library.Models;

namespace Library.DataAccess
{
    public interface IProvider<T>
    {
        public void Delete(string id);
        public List<T> GetAll();
        public T? GetById(string id);
        public void AddToFavourites(T item);
        public List<Cat> GetByBreed(string breedId);
        public List<T> GetFavouritesByUser(string username);
        public ApplicationUser? GetUser(string username);
    }
}
