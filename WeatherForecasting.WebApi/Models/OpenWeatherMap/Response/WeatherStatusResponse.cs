using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response
{
	public class WeatherStatusResponse
	{
		[JsonProperty("coord")]
		public Coordinates Coord { get; set; }

		[JsonProperty("weather")]
		public List<Weather> Weather { get; set; }

		[JsonProperty("base")]
		public string Base { get; set; }

		[JsonProperty("main")]
		public MainWeatherInformation MainInfo { get; set; }

		[JsonProperty("visibility")]
		public int Visibility { get; set; }

		[JsonProperty("wind")]
		public Wind Wind { get; set; }

		[JsonProperty("clouds")]
		public Clouds Clouds { get; set; }

		[JsonProperty("dt")]
		public long UnixTimestampUtc { get; set; }

		[JsonProperty("sys")]
		public SysInformation Sys { get; set; }

		[JsonProperty("rain")]
		public Rain Rain { get; set; }

		[JsonProperty("snow")]
		public Snow Snow { get; set; }


		[JsonProperty("timezone")]
		public int Timezone { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("cod")]
		public int Cod { get; set; }
	}

}
