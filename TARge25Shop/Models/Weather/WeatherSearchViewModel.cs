using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Models.Weather
{
    public class WeatherSearchViewModel
    {
        public string? City { get; set; }
        public WeatherApiResponseDto? Result { get; set; }
    }
}