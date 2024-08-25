namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record BaseWeatherRequest(decimal Latitude, decimal Longitude) : IQueryParametersRequest
{
	private const string DEFAULT_UNIT_SYSTEM = "metric";

	public virtual Dictionary<string, string> ToQueryParametersDictionary()
	{
		var result = new Dictionary<string, string>()
		{
			{ "lat", Latitude.ToString("N2") },
			{ "lon", Longitude.ToString("N2") },
			{ "units", DEFAULT_UNIT_SYSTEM }
		};


		return result;
	}
}
