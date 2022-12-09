using cat_app.Models;
using Library;

namespace cat_app.DataCache
{
    public class CatStore
    {
        private readonly CatApiClient<Cat> _apiClient;

        private List<Cat> Cats = new();

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
    }
}
