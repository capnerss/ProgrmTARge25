using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
    {
        public interface IWeatherServices
        {
            Task<WeatherApiResponseDto?> GetWeatherByCityAsync(string city);
            Task<WeatherApiResponseDto?> GetWeatherByCoordinatesAsync(double lat, double lon);
        }
    }