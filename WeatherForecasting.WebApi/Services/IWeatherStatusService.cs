using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public interface IWeatherStatusService
	{
		public Task<WeatherStatusResponse> GetWeatherStatusAsync(WeatherStatusRequest request);
	}
}
