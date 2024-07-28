using FluentValidation;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Validation
{
	public class WeatherForecastRequestValidator : AbstractValidator<WeatherForecastRequestByDate>
	{
		private const int MAX_DAYS_FORECAST = 5;
		public WeatherForecastRequestValidator(ITimeZoneService timeZoneService)
		{
			Include(new WeatherRequestValidator());
			var currentDateUtc = DateTime.UtcNow.Date;
			RuleFor(r => r.UtcDateTime.Date)
				.GreaterThanOrEqualTo(currentDateUtc)
				.WithMessage("Invalid date. Please provide date greater or equal to the current date.");
			RuleFor(r => r.UtcDateTime.Date)
				.LessThanOrEqualTo(currentDateUtc.AddDays(MAX_DAYS_FORECAST).Date)
				.WithMessage($"Invalid date. Please provide date within {MAX_DAYS_FORECAST} days range from the current date");
		}
	}
}
