using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public class WeatherForecastItem
{
	[JsonProperty("local_time")]
	public DateTime LocalTime { get; set; }
	[JsonProperty("utc_time")]
	public DateTime UtcTime { get; set; }
	[JsonProperty("summary")]
	public WeatherSummary WeatherSummary { get; set; }
	[JsonProperty("wind")]
	public Wind Wind { get; set; }
}
