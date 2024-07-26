using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private const int FORECAST_RESPONSE_ITEMS_LIMIT = 40;
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IValidator<BaseWeatherRequest> _validator;
		private readonly IWeatherForecastService _forecastService;
		private readonly IGeocodingService _geoService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService, IGeocodingService geoService, IValidator<BaseWeatherRequest> validator)
		{
			_forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
			_geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_validator = validator;
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherForecastByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<WeatherForecastResponse>>> ByCoordinates(decimal latitude, decimal longitude, int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByCoordinates)}");

			if (limit < 1 || limit > FORECAST_RESPONSE_ITEMS_LIMIT)
			{
				limit = Math.Clamp(limit, 1, FORECAST_RESPONSE_ITEMS_LIMIT);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var request = new WeatherForecastRequest(latitude, longitude, limit);
			var validationResult = _validator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var result = await _forecastService.GetWeatherForecastAsync(request);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByCoordinates)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<WeatherForecastResponse> ByLocation(string city, string state = "", string countryCode = "", int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByLocation)}");

			if (limit < 1 || limit > FORECAST_RESPONSE_ITEMS_LIMIT)
			{
				limit = Math.Clamp(limit, 1, FORECAST_RESPONSE_ITEMS_LIMIT);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var geocodingRequest = new GeocodingRequest(city, state, countryCode, Limit: 1);
			// TODO: add validation?
			var geocodingReponse = await _geoService.GetGeocodingCoordinatesByLocationAsync(geocodingRequest);

			var forecastRequest = new WeatherForecastRequest(geocodingReponse.Lat, geocodingReponse.Lon, limit);
			// TODO: add validation?
			var result = await _forecastService.GetWeatherForecastAsync(forecastRequest);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByLocation)}");
			return result;
		}
	}
}
