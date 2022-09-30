namespace Sample.Client.Mvc.Models;

/// <summary>
/// Create city model
/// </summary>
/// <param name="City">Name of the city</param>
/// <param name="Description">Description of the city</param>
public record CreateCityModel(string? City, string? Description);