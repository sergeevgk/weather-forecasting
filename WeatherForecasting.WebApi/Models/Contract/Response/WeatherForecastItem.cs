namespace WeatherForecasting.WebApi.Models.Contract.Response
{
	public class WeatherForecastItem
	{
		public DateTime LocalTime { get; set; }
		public WeatherSummary WeatherSummary { get; set; }
		public Wind Wind { get; set; }
	}
}
