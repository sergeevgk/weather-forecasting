using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response
{
	public class Snow
	{
		[JsonPropertyName("3h")]
		public double VolumeThreeHour { get; set; }

		[JsonPropertyName("1h")]
		public double VolumeOneHour { get; set; }
	}
}
