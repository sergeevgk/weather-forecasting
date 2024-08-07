﻿using GeoTimeZone;
using TimeZoneConverter;

namespace WeatherForecasting.WebApi.Services
{
	public class TimeZoneService: ITimeZoneService
	{
		public TimeZoneInfo GetLocalTimeZoneByCoordinates(decimal latitude, decimal longitude)
		{
			string timeZoneIana = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude).Result;
			TimeZoneInfo result = TZConvert.GetTimeZoneInfo(timeZoneIana);

			return result;
		}

		public DateTime GetLocalDateTimeByTimeZone(DateTime utcDateTime, TimeZoneInfo timeZone)
		{
			utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
			var result = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);

			return result;
		}


		public DateTime GetLocalDateTimeByCoordinates(DateTime utcDateTime, decimal latitude, decimal longitude)
		{
			string timeZoneIana = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude).Result;
			TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(timeZoneIana);
			var result = GetLocalDateTimeByTimeZone(utcDateTime, tzInfo);

			return result;
		}

		public DateTime GetUtcDateTimeByCoordinates(DateTime localDateTime, decimal latitude, decimal longitude)
		{
			string timeZoneIana = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude).Result;
			TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(timeZoneIana);
			DateTime unspecifiedLocalDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
			DateTime result = TimeZoneInfo.ConvertTimeToUtc(unspecifiedLocalDateTime, tzInfo);

			return result;
		}
	}
}
