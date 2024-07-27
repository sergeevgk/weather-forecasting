using Newtonsoft.Json.Converters;

namespace WeatherForecasting.WebApi.Helpers
{
	public class DateFormatConverter : IsoDateTimeConverter
	{
		public DateFormatConverter(string format)
		{
			DateTimeFormat = format;
		}
	}
}
