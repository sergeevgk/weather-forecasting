using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

namespace WeatherForecasting.WebApi.Services;

public interface IWeatherForecastService
{
	public Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request);
	public Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequestByDate request);
}
