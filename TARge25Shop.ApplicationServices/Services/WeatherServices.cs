using Nancy.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services

{
    public class WeatherServices : IWeatherServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public WeatherServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = "7a33fc13383be15f2b25ce32433bfd82";
            _baseUrl = "https://api.openweathermap.org/data/2.5/";
        }

        public async Task<WeatherApiResponseDto?> GetWeatherByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            try
            {
                var url = $"{_baseUrl}weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
                return await _httpClient.GetFromJsonAsync<WeatherApiResponseDto>(url);
            }
            catch (HttpRequestException)
            {
                // Returns null if the city isn't found (404) or network errors occur
                return null;
            }
        }

        public async Task<WeatherApiResponseDto?> GetWeatherByCoordinatesAsync(double lat, double lon)
        {
            try
            {
                var url = $"{_baseUrl}weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
                return await _httpClient.GetFromJsonAsync<WeatherApiResponseDto>(url);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}