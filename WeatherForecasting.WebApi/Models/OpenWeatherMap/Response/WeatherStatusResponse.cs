using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response
{
	public class WeatherStatusResponse
	{
		[JsonPropertyName("coord")]
		public Coordinates Coord { get; set; }

		[JsonPropertyName("weather")]
		public List<Weather> Weather { get; set; }

		[JsonPropertyName("base")]
		public string Base { get; set; }

		[JsonPropertyName("main")]
		public MainWeatherInformation MainInfo { get; set; }

		[JsonPropertyName("visibility")]
		public int Visibility { get; set; }

		[JsonPropertyName("wind")]
		public Wind Wind { get; set; }

		[JsonPropertyName("clouds")]
		public Clouds Clouds { get; set; }

		[JsonPropertyName("dt")]
		public long UnixTimestampUtc { get; set; }

		[JsonPropertyName("sys")]
		public SysInformation Sys { get; set; }

		[JsonProperty("rain")]
		public Rain Rain { get; set; }

		[JsonProperty("snow")]
		public Snow Snow { get; set; }


		[JsonPropertyName("timezone")]
		public int Timezone { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("cod")]
		public int Cod { get; set; }
	}

}
