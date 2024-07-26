using System.Drawing;
using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.Response
{
	public class WeatherForecastResponse
	{
		[JsonPropertyName("cod")]
		public string Cod { get; set; }

		[JsonPropertyName("message")]
		public int Message { get; set; }

		[JsonPropertyName("cnt")]
		public int Cnt { get; set; }

		[JsonPropertyName("list")]
		public List<WeatherForecast> List { get; set; }

		[JsonPropertyName("city")]
		public City City { get; set; }
	}
}
