using Library.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Library.Api
{
    public class CatApiClient<T>
    {
        private readonly HttpClient client;
        private readonly string ROOT_API_URL;
        private readonly string CAT_API_URL;
        private readonly string API_KEY;
        private const int limit = 20;

        public CatApiClient()
        {
            client = new();
            CAT_API_URL = string.Empty;
            API_KEY = string.Empty;
        }

        public CatApiClient(IConfiguration configuration)
        {
            client = new();
            API_KEY = configuration["CatApi:Key"];
            ROOT_API_URL = configuration["CatApi:Url"];
            CAT_API_URL = ROOT_API_URL + $"images/search?limit={limit}&api_key=" + API_KEY;
        }

        public async Task<List<T>?> GetCats()
        {
            var request = CreateRequest();
            try
            {
                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return CreateCatsFromJson(body);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Service unavailable! no internet connection");
            }
        }

        private HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CAT_API_URL)
            };
        }

        public List<T>? CreateCatsFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public List<Breed>? CreateBreedsFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<Breed>>(json);
        }

        public async Task<List<Breed>?> GetBreeds()
        {
            string url = $"{ROOT_API_URL}breeds";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            try
            {
                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return CreateBreedsFromJson(body);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Service unavailable! no internet connection");
            }
        }
    }
}
