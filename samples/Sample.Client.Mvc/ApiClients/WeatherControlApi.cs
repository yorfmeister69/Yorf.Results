using Newtonsoft.Json;
using Sample.Client.Mvc.Models;
using Yorf.Results.Core;
using Yorf.Results.AspNetCore;

namespace Sample.Client.Mvc.ApiClients;

public class WeatherControlApi : IWeatherControlApi
{
    private readonly HttpClient _httpClient;

    public WeatherControlApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all WeatherForecasts
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IEnumerable<WeatherForecast>>> GetAll(CancellationToken cancellationToken = default)
    {
        //make the HTTP request
        var httpResponse = await _httpClient.GetAsync("/WeatherForecast/GetWeatherForecast", cancellationToken);

        //use the Extension method to convert Http Response to Result<T>
        var result = await httpResponse.GetResult<IEnumerable<WeatherForecast>>(cancellationToken);

        return result;
    }

    /// <summary>
    /// Gets a WeatherForecast of a specific city
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public async Task<Result<WeatherForecast>> GetWeatherOfCity(string city, CancellationToken cancellationToken = default)
    {
        //make the HTTP request
        var httpResponse = await _httpClient.GetAsync("/WeatherForecast/GetWeatherForecastOfCity?city=" + city, cancellationToken);

        //use the Extension method to convert Http Response to Result<T>
        var result = await httpResponse.GetResult<WeatherForecast>(cancellationToken);

        return result;
    }

    /// <summary>
    /// Creates a city
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<Result> CreateCity(CreateCityModel model, CancellationToken cancellationToken = default)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/WeatherForecast/CreateCity")
        {
            Content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json")
        };

        //make the HTTP request
        var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);

        //use the Extension method to convert Http Response to Result
        var result = await httpResponse.GetResult(cancellationToken);

        return result;
    }
}