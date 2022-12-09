using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Library
{
    public class CatApiClient<T>
    {
        private readonly HttpClient client;
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
            CAT_API_URL = configuration["CatApi:Url"] + $"?limit={limit}&api_key=" + API_KEY;
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
            }catch(HttpRequestException e)
            {
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
    }
}
