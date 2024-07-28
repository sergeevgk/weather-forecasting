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
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IValidator<BaseWeatherRequest> _weatherValidator;
		private readonly IValidator<WeatherForecastRequestByDate> _weatherDateValidator;
		private readonly IValidator<GeocodingRequest> _geoValidator;
		private readonly IWeatherForecastService _forecastService;
		private readonly IGeocodingService _geoService;
		private readonly ITimeZoneService _timeService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService forecastService, IGeocodingService geoService, IValidator<BaseWeatherRequest> weatherValidator, IValidator<WeatherForecastRequestByDate> weatherDateValidator, IValidator<GeocodingRequest> geoValidator, ITimeZoneService timeService)
		{
			_forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
			_geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_weatherValidator = weatherValidator ?? throw new ArgumentNullException(nameof(weatherValidator));
			_weatherDateValidator = weatherDateValidator ?? throw new ArgumentNullException(nameof(weatherDateValidator));
			_geoValidator = geoValidator ?? throw new ArgumentNullException(nameof(geoValidator));
			_timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinatesAndDate", Name = "GetWeatherForecastByCoordinatesAndDate")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherForecastResponse>>> ByCoordinatesAndDate(decimal latitude, decimal longitude, string date = "")
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByCoordinatesAndDate)}");

			DateTime requestDate = DateTime.MinValue;
			if (!string.IsNullOrWhiteSpace(date))
			{
				var requestDateIsParsed = DateTime.TryParse(date, null, System.Globalization.DateTimeStyles.RoundtripKind, out requestDate);
				if (!requestDateIsParsed)
				{
					var badRequestResult = TypedResults.BadRequest($"The provided parameter is invalid: {nameof(date)} [{date}]. Please provide a valid date in format DD-MM-YYYY or omit this parameter.");
					return badRequestResult;
				}
			}

			var utcRequestDateTime = _timeService.GetUtcDateTimeByCoordinates(requestDate, latitude, longitude);
			var request = new WeatherForecastRequestByDate(latitude, longitude, requestDate, utcRequestDateTime);
			var validationResult = _weatherDateValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			var result = await _forecastService.GetWeatherForecastAsync(request);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByCoordinatesAndDate)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinatesLimited", Name = "GetWeatherForecastByCoordinatesAndLimit")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherForecastResponse>>> ByCoordinatesLimited(decimal latitude, decimal longitude, int limit = Constants.ForecastResponseItemsLimit)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByCoordinatesAndDate)}");

			if (limit < 1 || limit > Constants.ForecastResponseItemsLimit)
			{
				limit = Math.Clamp(limit, 1, Constants.ForecastResponseItemsLimit);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var request = new WeatherForecastRequest(latitude, longitude, limit);
			var validationResult = _weatherValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			var result = await _forecastService.GetWeatherForecastAsync(request);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByCoordinatesAndDate)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherForecastByDate")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherForecastResponse>>> ByLocationAndDate(string city, string stateCode = "", string countryCode = "", string date = "")
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByLocationAndDate)}");

			DateTime requestDate = DateTime.MinValue;
			if (!string.IsNullOrWhiteSpace(date))
			{
				var requestDateIsParsed = DateTime.TryParse(date, null, System.Globalization.DateTimeStyles.RoundtripKind, out requestDate);
				if (!requestDateIsParsed)
				{
					var badRequestResult = TypedResults.BadRequest($"The provided parameter is invalid: {nameof(date)} [{date}]. Please provide a valid date in format DD-MM-YYYY or omit this parameter.");
					return badRequestResult;
				}
			}

			var geocodingRequest = new GeocodingRequest(city, stateCode, countryCode, Limit: 1);
			var geoValidationResult = _geoValidator.Validate(geocodingRequest);
			if (!geoValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", geoValidationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var geocodingResponse = await _geoService.GetGeocodingCoordinatesByLocationAsync(geocodingRequest);

			var utcRequestDateTime = _timeService.GetUtcDateTimeByCoordinates(requestDate, geocodingResponse.Lat, geocodingResponse.Lon);
			var forecastRequest = new WeatherForecastRequestByDate(geocodingResponse.Lat, geocodingResponse.Lon, requestDate, utcRequestDateTime);
			var weatherValidationResult = _weatherDateValidator.Validate(forecastRequest);
			if (!weatherValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", weatherValidationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			var result = await _forecastService.GetWeatherForecastAsync(forecastRequest);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByLocationAndDate)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByLocationlimited", Name = "GetWeatherForecastLimited")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherForecastResponse>>> ByLocationLimited(string city, string stateCode = "", string countryCode = "", string date = "", int limit = Constants.ForecastResponseItemsLimit)
		{
			_logger.LogInformation($"{nameof(WeatherForecastController)}. Start request {nameof(ByLocationLimited)}");

			if (limit < 1 || limit > Constants.ForecastResponseItemsLimit)
			{
				limit = Math.Clamp(limit, 1, Constants.ForecastResponseItemsLimit);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var geocodingRequest = new GeocodingRequest(city, stateCode, countryCode, Limit: 1);
			var geoValidationResult = _geoValidator.Validate(geocodingRequest);
			if (!geoValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", geoValidationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var geocodingResponse = await _geoService.GetGeocodingCoordinatesByLocationAsync(geocodingRequest);

			var forecastRequest = new WeatherForecastRequest(geocodingResponse.Lat, geocodingResponse.Lon, limit);
			var weatherValidationResult = _weatherValidator.Validate(forecastRequest);
			if (!geoValidationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", geoValidationResult.Errors);
				var badRequestResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badRequestResult;
			}

			var result = await _forecastService.GetWeatherForecastAsync(forecastRequest);

			_logger.LogInformation($"{nameof(WeatherForecastController)}. Finish request {nameof(ByLocationLimited)}");
			return TypedResults.Ok(result);
		}

	}
}
