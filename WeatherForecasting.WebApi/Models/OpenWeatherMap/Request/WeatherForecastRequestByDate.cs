using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record WeatherForecastRequestByDate(decimal Latitude, decimal Longitude, DateOnly Date) : BaseWeatherRequest(Latitude, Longitude)
{
	public DateTime UtcDateTime => TimeZoneService.GetUtcDateTimeByCoordinates(Date.ToDateTime(new TimeOnly(0)), Latitude, Longitude);

	public override Dictionary<string, string> ToQueryParametersDictionary()
	{
		var result = base.ToQueryParametersDictionary();

		return result;
	}
}
