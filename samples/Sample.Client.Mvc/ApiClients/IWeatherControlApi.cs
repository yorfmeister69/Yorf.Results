using Sample.Client.Mvc.Models;
using Yorf.Results.Core;

namespace Sample.Client.Mvc.ApiClients
{
    public interface IWeatherControlApi
    {
        Task<Result> CreateCity(CreateCityModel model, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<WeatherForecast>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<WeatherForecast>> GetWeatherOfCity(string city, CancellationToken cancellationToken = default);
    }
}