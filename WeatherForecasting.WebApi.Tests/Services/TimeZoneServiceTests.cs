using System.Globalization;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Tests.Services
{
	[TestFixture]
	public class TimeZoneServiceTests
	{

		[TestCase("2024-07-25T12:00:00Z", 44.8178, 20.4569, "2024-07-25T14:00:00", TestName = "Convert from UTC to UTC+2")]
		[TestCase("2024-12-25T12:00:00Z", 34.0522, -118.2437, "2024-12-25T04:00:00", TestName = "Convert from UTC to UTC-7")]
		[TestCase("2024-03-25T12:00:00Z", 35.6895, 139.6917, "2024-03-25T21:00:00", TestName = "Convert from UTC to UTC+9")]
		public void GetLocalDateTimeByCoordinates_ValidInput_ReturnsCorrectLocalTime(string utcDateTimeString, decimal latitude, decimal longitude, string expectedLocalDateTimeString)
		{
			DateTime utcDateTime = DateTime.Parse(utcDateTimeString, null, DateTimeStyles.RoundtripKind);
			DateTime expectedLocalDateTime = DateTime.Parse(expectedLocalDateTimeString);

			var service = new TimeZoneService();
			DateTime actualLocalDateTime = service.GetLocalDateTimeByCoordinates(utcDateTime, latitude, longitude);

			Assert.AreEqual(expectedLocalDateTime, actualLocalDateTime);
		}

		[TestCase("2024-07-25T14:00:00", 44.8178, 20.4569, "2024-07-25T12:00:00Z", TestName = "Convert from UTC+2 to UTC")]
		[TestCase("2024-12-25T04:00:00", 34.0522, -118.2437, "2024-12-25T12:00:00Z", TestName = "Convert from UTC-7 to UTC")]
		[TestCase("2024-03-25T21:00:00", 35.6895, 139.6917, "2024-03-25T12:00:00Z", TestName = "Convert from UTC+9 to UTC")]
		public void GetUtcDateTimeByCoordinates_ValidInput_ReturnsCorrectUtcTime(string localDateTimeString, decimal latitude, decimal longitude, string expectedUtcDateTimeString)
		{
			DateTime localDateTime = DateTime.Parse(localDateTimeString);
			DateTime expectedUtcDateTime = DateTime.Parse(expectedUtcDateTimeString, null, DateTimeStyles.RoundtripKind);

			var service = new TimeZoneService();
			DateTime actualUtcDateTime = service.GetUtcDateTimeByCoordinates(localDateTime, latitude, longitude);

			Assert.AreEqual(expectedUtcDateTime, actualUtcDateTime);
		}
	}
}
