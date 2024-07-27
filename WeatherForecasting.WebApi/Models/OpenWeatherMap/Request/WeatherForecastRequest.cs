using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Request
{
	public record WeatherForecastRequest(decimal Latitude, decimal Longitude, int ForecastsCount, DateOnly Date, bool UseDateFilter) : BaseWeatherRequest(Latitude, Longitude)
	{
		public DateTime UtcDateTime => TimeZoneService.GetUtcDateTimeByCoordinates(Date.ToDateTime(new TimeOnly(0)), Latitude, Longitude);

		public override Dictionary<string, string> ToQueryParametersDictionary()
		{
			var result = base.ToQueryParametersDictionary();
			result.Add("cnt", ForecastsCount.ToString());

			return result;
		}
	}
}
