namespace WeatherForecasting.WebApi.Services
{
	public interface ITimeZoneService
	{
		public TimeZoneInfo GetLocalTimeZoneByCoordinates(decimal latitude, decimal longitude);
		public DateTime GetLocalDateTimeByTimeZone(DateTime utcDateTime, TimeZoneInfo timeZone);
		public DateTime GetLocalDateTimeByCoordinates(DateTime utcDateTime, decimal latitude, decimal longitude);
		public DateTime GetUtcDateTimeByCoordinates(DateTime localDateTime, decimal latitude, decimal longitude);
	}
}
