using Yorf.Results.Core;

namespace Sample.Api.Services
{
    public interface IWeahtherForcastService
    {
        Result CreateCity(CreateCityModel model);
        Result<IEnumerable<WeatherForecast>> GetAll();
        Result<WeatherForecast> GetWeatherOfCity(string city);
    }
}