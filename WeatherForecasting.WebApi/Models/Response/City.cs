using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.Response
{
	public class City
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("coord")]
		public Coordinates Coordinates { get; set; }

		[JsonPropertyName("country")]
		public string Country { get; set; }

		[JsonPropertyName("population")]
		public int Population { get; set; }

		[JsonPropertyName("timezone")]
		public int Timezone { get; set; }

		[JsonPropertyName("sunrise")]
		public long Sunrise { get; set; }

		[JsonPropertyName("sunset")]
		public long Sunset { get; set; }
	}
}
