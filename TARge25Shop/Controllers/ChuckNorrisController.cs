using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.ChuckNorris;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.ChuckNorris;

namespace TARge25Shop.Controllers
{
    public class ChuckNorrisController : Controller
    {
        private readonly IChuckNorrisServices _services;
        public ChuckNorrisController(IChuckNorrisServices services)
        {
            _services = services;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchChuckNorrisJoke()
        {
            return RedirectToAction(nameof(GetRandomJoke));
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomJoke()
        {
            var joke = await _services.ChuckNorrisResultHttp();

            var model = new JokeViewModel
            {
                Categories = joke.Categories,
                CreatedAt = joke.CreatedAt,
                IconUrl = joke.IconUrl,
                Id = joke.Id,
                UpdatedAt = joke.UpdatedAt,
                Url = joke.Url,
                Value = joke.Value
            };

            return View("Joke", model);
        }
    }
}
