using Newtonsoft.Json;
using WeatherForecasting.WebApi.Helpers;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response
{
	public class WeatherForecastItem
	{
		[JsonProperty("dt")]
		public long Dt { get; set; }

		[JsonProperty("main")]
		public MainWeatherInformation Main { get; set; }

		[JsonProperty("weather")]
		public List<Weather> Weather { get; set; }

		[JsonProperty("clouds")]
		public Clouds Clouds { get; set; }

		[JsonProperty("wind")]
		public Wind Wind { get; set; }

		[JsonProperty("visibility")]
		public int Visibility { get; set; }

		[JsonProperty("pop")]
		public double Pop { get; set; }

		[JsonProperty("rain")]
		public Rain Rain { get; set; }

		[JsonProperty("sys")]
		public SysInformation Sys { get; set; }

		[JsonProperty("dt_txt")]
		[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
		public DateTime ForecastDateTime { get; set; }
	}
}
