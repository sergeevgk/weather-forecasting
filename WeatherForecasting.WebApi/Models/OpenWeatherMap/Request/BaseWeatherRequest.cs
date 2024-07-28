namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record BaseWeatherRequest(decimal Latitude, decimal Longitude) : IQueryParametersRequest
{
	public virtual Dictionary<string, string> ToQueryParametersDictionary()
	{
		var result = new Dictionary<string, string>()
		{
			{ "lat", Latitude.ToString("N2") },
			{ "lon", Longitude.ToString("N2") }
		};


		return result;
	}
}
