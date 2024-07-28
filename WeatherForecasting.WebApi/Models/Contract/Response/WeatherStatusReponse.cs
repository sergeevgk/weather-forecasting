using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public class WeatherStatusResponse
{
	[JsonProperty("status_time")]
	public DateTime StatusLocalTime { get; set; }

	[JsonProperty("status_time_utc")]
	public DateTime StatusUtcTime { get; set; }

	[JsonProperty("place")]
	public Place Place { get; set; }

	[JsonProperty("weather_summary")]
	public WeatherSummary WeatherSummary { get; set; }

	[JsonProperty("wind")]
	public Wind Wind { get; set; }
}
