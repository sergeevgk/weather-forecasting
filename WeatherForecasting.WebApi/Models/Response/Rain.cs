using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.Response
{
	public class Rain
	{
		[JsonPropertyName("3h")]
		public double ThreeHour { get; set; }
	}
}
