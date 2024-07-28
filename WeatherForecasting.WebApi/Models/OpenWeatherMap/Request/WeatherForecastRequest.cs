namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record WeatherForecastRequest(decimal Latitude, decimal Longitude, int ForecastsLimit) : BaseWeatherRequest(Latitude, Longitude)
{
	public override Dictionary<string, string> ToQueryParametersDictionary()
	{
		var result = base.ToQueryParametersDictionary();
		result.Add("cnt", ForecastsLimit.ToString());

		return result;
	}
}
