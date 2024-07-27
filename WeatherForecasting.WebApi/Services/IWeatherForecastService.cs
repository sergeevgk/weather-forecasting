using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

namespace WeatherForecasting.WebApi.Services;

public interface IWeatherForecastService
{
	public Task<WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request);
}
