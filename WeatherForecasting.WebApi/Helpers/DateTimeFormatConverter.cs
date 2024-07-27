using Newtonsoft.Json.Converters;

namespace WeatherForecasting.WebApi.Helpers
{
	public class DateTimeFormatConverter : IsoDateTimeConverter
	{
		public DateTimeFormatConverter(string format)
		{
			DateTimeFormat = format;
		}
	}
}
