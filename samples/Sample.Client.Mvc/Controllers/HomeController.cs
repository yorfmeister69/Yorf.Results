using Microsoft.AspNetCore.Mvc;
using Sample.Client.Mvc.ApiClients;
using Sample.Client.Mvc.Models;
using System.Diagnostics;
using Yorf.Results.AspNetCore;

namespace Sample.Client.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherControlApi _weatherControlApi;

        public HomeController(ILogger<HomeController> logger, IWeatherControlApi weatherControlApi)
        {
            _logger = logger;
            _weatherControlApi = weatherControlApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetAll()
        {
            //get the response from API wrapped in Result<>
            var result = await _weatherControlApi.GetAll();

            //call the .Handle() extension to convert it to an IActionResult
            return result.Handle(this);
        }

        public async Task<IActionResult> GetWeatherOfCity(string city)
        {
            //get the response from API wrapped in Result<>
            var result = await _weatherControlApi.GetWeatherOfCity(city);

            //call the .Handle() extension to convert it to an IActionResult
            return result.Handle(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CreateCityModel model)
        {
            //get the response from API wrapped in Result<>
            var result = await _weatherControlApi.CreateCity(model);

            //call the .Handle() extension to convert it to an IActionResult
            return result.Handle(this);
        }
    }
}