using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public class GeocodingService : IGeocodingService
	{
		public GeocodingResponse GetCoordinatesByLocation(GeocodingRequest request)
		{
			var response = new GeocodingResponse() { Lat = 10.00m, Lon = 10.00m };

			return response;
		}
	}
}
