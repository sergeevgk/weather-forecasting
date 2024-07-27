namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class GeocodingResponse : Coordinates
{
	public string Name { get; set; }
	public string Country { get; set; }
	public string State { get; set; }
	public Dictionary<string, string> LocalNames { get; set; }
}
