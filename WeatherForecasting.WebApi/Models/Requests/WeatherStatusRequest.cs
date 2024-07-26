namespace WeatherForecasting.WebApi.Models.Requests
{
	public record WeatherStatusRequest(decimal Latitude, decimal Longitude) : BaseWeatherRequest(Latitude, Longitude);
}
