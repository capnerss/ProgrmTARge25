using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Weather;

namespace TARge25Shop.Controllers

{
    public class WeatherController : Controller
    {
        private readonly IWeatherServices _weatherServices;

        public WeatherController(IWeatherServices weatherServices)
        {
            _weatherServices = weatherServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new WeatherSearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(WeatherSearchViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.City))
            {
                ViewBag.EmptyMessage = "Lisa linna nime";
                return View(model);
            }

            var result = await _weatherServices.GetWeatherByCityAsync(model.City);

            if (result == null)
            {
                ViewBag.EmptyMessage = $"Linn \"{model.City}\" ei eksisteri.";
                return View(model);
            }

            model.Result = result;
            return View(model);
        }
    }
}