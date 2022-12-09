using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Library
{
    public class CatApiClient<T>
    {
        private readonly HttpClient client;
        private readonly string CAT_API_URL;
        private readonly string API_KEY;

        public CatApiClient()
        {
            client = new();
            CAT_API_URL = string.Empty;
            API_KEY = string.Empty;
        }

        public CatApiClient(IConfiguration configuration)
        {
            client = new();
            CAT_API_URL = configuration["CatApi:Url"];
            API_KEY = configuration["CatApi:Key"];
        }

        public async Task<List<T>?> GetCats()
        {
            var request = CreateRequest();
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return CreateCatsFromJson(body);
        }

        private HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(CAT_API_URL),
                Headers =
            {
                { "x-api-key", API_KEY },
            },
            };
        }

        public List<T>? CreateCatsFromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
