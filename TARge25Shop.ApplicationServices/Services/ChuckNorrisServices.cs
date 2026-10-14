using Nancy.Json;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using System.Net;
using System.Text.Json;



namespace TARge25Shop.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
        private readonly HttpClient _httpClient;
        public ChuckNorrisServices(HttpClient httpClient) 
        {  _httpClient = httpClient; }
        public async Task<ChuckNorrisRootDto> ChuckNorrisResultHttp()
        {
            var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var joke = JsonSerializer.Deserialize<ChuckNorrisRootDto>(json, options);

            return joke!;
            
        }
    }
}