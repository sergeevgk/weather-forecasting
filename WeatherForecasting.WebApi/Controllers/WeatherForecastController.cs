using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private const int FORECAST_RESPONSE_ITEMS_LIMIT = 40;
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IValidator<BaseWeatherRequest> _weatherValidator;
		private readonly IValidator<GeocodingRequest> _geoValidator;
		private readonly IWeatherForecastService _forecastService;
		private readonly IGeocodingService _geoService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService, IGeocodingService geoService, IValidator<BaseWeatherRequest> weatherValidator, IValidator<GeocodingRequest> geoValidator)
		{
			_forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
			_geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_weatherValidator = weatherValidator;
			_geoValidator = geoValidator;
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherForecastByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<WeatherForecastResponse>>> ByCoordinates(decimal latitude, decimal longitude, string date = "", int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByCoordinates)}");

			DateOnly requestDate = DateOnly.MinValue;
			var requestDateCanBeParsed = !string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out requestDate);
			if (!requestDateCanBeParsed)
			{
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {nameof(date)} [{date}]. Please provide a valid date in format DD-MM-YYYY or omit the date parameter.");
				return badRequestResult;
			}

			if (limit < 1 || limit > FORECAST_RESPONSE_ITEMS_LIMIT)
			{
				limit = Math.Clamp(limit, 1, FORECAST_RESPONSE_ITEMS_LIMIT);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var request = new WeatherForecastRequest(latitude, longitude, limit, requestDate, false);
			var validationResult = _weatherValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			if (requestDateCanBeParsed)
			{
				limit = FORECAST_RESPONSE_ITEMS_LIMIT;
				request = request with { ForecastsCount = limit, UseDateFilter = true };
				_logger.LogInformation($"Request limit was reset to {limit} because a valid date parameter was provided. Calculating forecast for the date [{requestDate}].");
			}

			var result = await _forecastService.GetWeatherForecastAsync(request);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByCoordinates)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<Results<BadRequest<string>, Ok<WeatherForecastResponse>>> ByLocation(string city, string state = "", string countryCode = "", string date = "", int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByLocation)}");

			DateOnly requestDate = DateOnly.MinValue;
			var requestDateCanBeParsed = !string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out requestDate);
			if (!requestDateCanBeParsed)
			{
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {nameof(date)} [{date}]. Please provide a valid date in format DD-MM-YYYY or omit the date parameter.");
				return badRequestResult;
			}

			if (limit < 1 || limit > FORECAST_RESPONSE_ITEMS_LIMIT)
			{
				limit = Math.Clamp(limit, 1, FORECAST_RESPONSE_ITEMS_LIMIT);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var geocodingRequest = new GeocodingRequest(city, state, countryCode, Limit: 1);
			var geoValidationResult = _geoValidator.Validate(geocodingRequest);
			if (!geoValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", geoValidationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var geocodingReponse = await _geoService.GetGeocodingCoordinatesByLocationAsync(geocodingRequest);

			var forecastRequest = new WeatherForecastRequest(geocodingReponse.Lat, geocodingReponse.Lon, limit, requestDate, false);
			var weatherValidationResult = _weatherValidator.Validate(forecastRequest);
			if (!geoValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", geoValidationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			if (requestDateCanBeParsed)
			{
				limit = FORECAST_RESPONSE_ITEMS_LIMIT;
				forecastRequest = forecastRequest with { ForecastsCount = limit, UseDateFilter = true };
				_logger.LogInformation($"Request limit was reset to {limit} because a valid date parameter was provided. Calculating forecast for the date [{requestDate}].");
			}

			var result = await _forecastService.GetWeatherForecastAsync(forecastRequest);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByLocation)}");
			return TypedResults.Ok(result);
		}
	}
}
