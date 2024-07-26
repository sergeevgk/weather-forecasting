using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public interface IGeocodingService
	{
		public Task<GeocodingResponse> GetGeocodingCoordinatesByLocationAsync(GeocodingRequest request);
	}
}
