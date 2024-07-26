using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public interface IWeatherForecastService
	{
		public Task<WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request);
	}
}
