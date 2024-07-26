using FluentValidation;
using WeatherForecasting.WebApi.Models.Requests;

namespace WeatherForecasting.WebApi.Validation
{
	public class WeatherRequestValidator : AbstractValidator<BaseWeatherRequest>
	{
		public WeatherRequestValidator()
		{
			RuleFor(r => r.Latitude).InclusiveBetween(-90.00m, 90.00m);
			RuleFor(r => r.Longitude).GreaterThanOrEqualTo(-180.00m).LessThan(180.00m);
		}
	}
}
