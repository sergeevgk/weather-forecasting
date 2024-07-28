namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

public record WeatherStatusRequest(decimal Latitude, decimal Longitude) : BaseWeatherRequest(Latitude, Longitude);
