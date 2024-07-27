using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

namespace WeatherForecasting.WebApi.Services;

public interface IWeatherStatusService
{
	public Task<Models.Contract.Response.WeatherStatusResponse> GetWeatherStatusAsync(WeatherStatusRequest request);
}
