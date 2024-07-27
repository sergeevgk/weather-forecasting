using FluentValidation;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

namespace WeatherForecasting.WebApi.Validation
{
	public class GeocodingRequestValidation : AbstractValidator<GeocodingRequest>
	{
		public GeocodingRequestValidation()
		{
			RuleFor(r => r.CityName).Must(cityString => cityString.All(x => char.IsLetter(x) || char.IsWhiteSpace(x))).WithMessage("City name is invalid.");
			RuleFor(r => r.StateCode).Must(state => string.IsNullOrEmpty(state) || state.All(x => char.IsLetter(x) || char.IsWhiteSpace(x))).WithMessage("State is invalid.");
			RuleFor(r => r.CountryCode).Must(country => string.IsNullOrEmpty(country) || country.All(x => char.IsLetter(x) || char.IsWhiteSpace(x))).WithMessage("Country code is invalid.");
		}
	}
}
