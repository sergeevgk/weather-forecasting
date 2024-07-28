using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response
{
	public class WeatherForecastResponse
	{
		[JsonProperty("cod")]
		public string Cod { get; set; }

		[JsonProperty("message")]
		public int Message { get; set; }

		[JsonProperty("cnt")]
		public int Cnt { get; set; }

		[JsonProperty("list")]
		public List<WeatherForecastItem> List { get; set; }

		[JsonProperty("city")]
		public City City { get; set; }
	}
}
