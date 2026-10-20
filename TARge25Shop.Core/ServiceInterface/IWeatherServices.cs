using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
    {
        public interface IWeatherServices
        {
            /// <summary>
            /// Fetches full current weather details for a given city name.
            /// </summary>
            /// <param name="city">Name of the city (e.g., "Valga", "Tallinn").</param>
            /// <returns>WeatherApiResponseDto if found, otherwise null.</returns>
            Task<WeatherApiResponseDto?> GetWeatherByCityAsync(string city);

            /// <summary>
            /// Fetches weather details based on geographical coordinates.
            /// </summary>
            /// <param name="lat">Latitude</param>
            /// <param name="lon">Longitude</param>
            /// <returns>WeatherApiResponseDto if found, otherwise null.</returns>
            Task<WeatherApiResponseDto?> GetWeatherByCoordinatesAsync(double lat, double lon);
        }
    }