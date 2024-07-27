using GeoTimeZone;
using TimeZoneConverter;

namespace WeatherForecasting.WebApi.Services
{
	public static class TimeZoneService
	{
		public static DateTime GetLocalDateTimeByCoordinates(DateTime utcDateTime, decimal latitude, decimal longitude)
		{
			string timeZoneIana = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude).Result;
			TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(timeZoneIana);
			var result = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tzInfo);

			return result;
		}

		public static DateTime GetUtcDateTimeByCoordinates(DateTime localDateTime, decimal latitude, decimal longitude)
		{
			string timeZoneIana = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude).Result;
			TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(timeZoneIana);
			DateTime unspecifiedLocalDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
			DateTime result = TimeZoneInfo.ConvertTimeToUtc(unspecifiedLocalDateTime, tzInfo);

			return result;
		}
	}
}
