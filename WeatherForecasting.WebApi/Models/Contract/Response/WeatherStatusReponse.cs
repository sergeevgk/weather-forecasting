namespace WeatherForecasting.WebApi.Models.Contract.Response;

using System;
using System.Text.Json.Serialization;

public class WeatherStatusResponse
{
	[JsonPropertyName("status_time")]
	public DateTime StatusLocalTime { get; set; }

	[JsonPropertyName("place")]
	public Place Place { get; set; }

	[JsonPropertyName("weather_summary")]
	public WeatherSummary WeatherSummary { get; set; }

	[JsonPropertyName("wind")]
	public Wind Wind { get; set; }
}
