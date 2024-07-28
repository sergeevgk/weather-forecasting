namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record WeatherForecastRequestByDate(decimal Latitude, decimal Longitude, DateTime LocalDateTime, DateTime UtcDateTime) : BaseWeatherRequest(Latitude, Longitude)
{
	public override Dictionary<string, string> ToQueryParametersDictionary()
	{
		var result = base.ToQueryParametersDictionary();

		return result;
	}
}
