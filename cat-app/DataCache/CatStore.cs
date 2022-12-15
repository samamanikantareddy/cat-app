using Library.Models;
using Library.Api;

namespace cat_app.DataCache
{
    public class CatStore
    {
        private readonly CatApiClient<Cat> _apiClient;

        private List<Cat> Cats = new();
        private List<Breed> Breeds = new();

        public CatStore(CatApiClient<Cat> apiClient)
        {
            _apiClient = apiClient;
        }

        public List<Cat> GetCats()
        {
            if (Cats.Count == 0)
            {
                Cats = _apiClient.GetCats()!.Result!.ToList();
            }
            return Cats;
        }

        public List<Breed> GetBreeds()
        {
            if (Breeds.Count == 0)
            {
                Cats = _apiClient.GetBreeds()!.Result!.ToList();
            }
            return Breeds;
        }
    }
}
