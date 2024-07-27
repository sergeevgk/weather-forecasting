using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

namespace WeatherForecasting.WebApi.Services;

public interface IGeocodingService
{
	public Task<GeocodingResponse> GetGeocodingCoordinatesByLocationAsync(GeocodingRequest request);
}
