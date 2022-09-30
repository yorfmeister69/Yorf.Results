using Microsoft.AspNetCore.Mvc;
using Sample.Api.Services;
using Yorf.Results.AspNetCore;

namespace Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeahtherForcastService _weahtherForcastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeahtherForcastService weahtherForcastService)
        {
            _logger = logger;
            _weahtherForcastService = weahtherForcastService;
        }

        [HttpGet("GetWeatherForecast")]
        public IActionResult Get()
        {
            //gets the Result<IEnumerable<WeatherForecast>> object
            var result = _weahtherForcastService.GetAll();

            //Handle extension method automatically convert the Result to approriate Http response type based on the status.
            return result.Handle(this);
        }


        [HttpGet("GetWeatherForecastOfCity")]
        public IActionResult GetOfCity(string city)
        {
            //gets the Result<WeatherForecast> object
            var result = _weahtherForcastService.GetWeatherOfCity(city);

            //Handle extension method automatically convert the Result to approriate Http response type based on the status.
            return result.Handle(this);
        }

        [HttpPost("CreateCity")]
        public IActionResult CreateCity(CreateCityModel model)
        {
            //gets the create city result object
            var result = _weahtherForcastService.CreateCity(model);

            //Handle extension method automatically convert the Result to approriate Http response type based on the status.
            return result.Handle(this);
        }
    }
}