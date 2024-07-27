namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public interface IQueryParametersRequest
{
	public Dictionary<string, string> ToQueryParametersDictionary();
}
