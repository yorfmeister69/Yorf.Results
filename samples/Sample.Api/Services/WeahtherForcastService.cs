using Yorf.Results.Core;

namespace Sample.Api.Services;

public class WeahtherForcastService : IWeahtherForcastService
{
    private static readonly string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    /// <summary>
    /// Get all WeatherForecasts
    /// </summary>
    /// <returns></returns>
    public Result<IEnumerable<WeatherForecast>> GetAll()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = _summaries[Random.Shared.Next(_summaries.Length)]
        }).ToArray();
    }

    /// <summary>
    /// Gets a WeatherForecast of a specific city
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public Result<WeatherForecast> GetWeatherOfCity(string city)
    {
        //for testing, if we get the city called "London" we will return a failed result. (To simulate failure)
        if (city.Equals("london", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Error("An error has occured. London bridges are falling down.");
        }

        //if city equals new york, we'll return a not found result.
        if (city.Equals("new york", StringComparison.OrdinalIgnoreCase))
        {
            return Result.NotFound();
        }

        //Result.Success is implicitly convertible between any objects. So you do not need to explicitly return Result.Success();
        return new WeatherForecast()
        {
            Date = DateTime.Now,
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = _summaries[Random.Shared.Next(_summaries.Length)]
        };
    }

    /// <summary>
    /// Creates a city
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Result CreateCity(CreateCityModel model)
    {
        //check for empty city name
        if (string.IsNullOrWhiteSpace(model.City))
            return Result.Invalid(nameof(model.City), "City cannot be empty!");

        //we will not allow to create city with name "Tokyo"
        if (model.City.Equals("tokyo", StringComparison.OrdinalIgnoreCase))
            return Result.Invalid(nameof(model.City), "This is not a valid city!");

        //return empty success result indicating the city has been created
        return Result.Success();
    }
}