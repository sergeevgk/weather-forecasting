using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

namespace WeatherForecasting.WebApi.Services;

public interface IWeatherStatusService
{
	public Task<WeatherStatusResponse> GetWeatherStatusAsync(WeatherStatusRequest request);
}
