using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.Response
{
	public class WeatherForecast
	{
		[JsonPropertyName("dt")]
		public long Dt { get; set; }

		[JsonPropertyName("main")]
		public MainWeatherInformation Main { get; set; }

		[JsonPropertyName("weather")]
		public List<Weather> Weather { get; set; }

		[JsonPropertyName("clouds")]
		public Clouds Clouds { get; set; }

		[JsonPropertyName("wind")]
		public Wind Wind { get; set; }

		[JsonPropertyName("visibility")]
		public int Visibility { get; set; }

		[JsonPropertyName("pop")]
		public double Pop { get; set; }

		[JsonPropertyName("rain")]
		public Rain Rain { get; set; }

		[JsonPropertyName("sys")]
		public SysInformation Sys { get; set; }

		[JsonPropertyName("dt_txt")]
		public string DtTxt { get; set; }
	}
}
