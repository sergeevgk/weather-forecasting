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
	public class WeatherStatusController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IValidator<BaseWeatherRequest> _weatherValidator;
		private readonly IValidator<GeocodingRequest> _geoValidator;
		private readonly IWeatherStatusService _statusService;
		private readonly IGeocodingService _geoService;

		public WeatherStatusController(ILogger<WeatherForecastController> logger, IWeatherStatusService statusService, IGeocodingService geoService, IValidator<BaseWeatherRequest> weatherValidator, IValidator<GeocodingRequest> geoValidator)
		{
			_statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
			_geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_weatherValidator = weatherValidator;
			_geoValidator = geoValidator;
		}

		[ProducesResponseType(typeof(WeatherStatusResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherStatusByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherStatusResponse>>> ByCoordinates(decimal latitude, decimal longitude)
		{
			_logger.LogInformation($"{nameof(WeatherStatusController)}. Start request {nameof(ByCoordinates)}");

			var request = new WeatherStatusRequest(latitude, longitude);
			var validationResult = _weatherValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var result = await _statusService.GetWeatherStatusAsync(request);

			_logger.LogInformation($"{nameof(WeatherStatusController)}. Finish request {nameof(ByCoordinates)}");
			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherStatusResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherStatus")]
		public async Task<Results<BadRequest<string>, Ok<Models.Contract.Response.WeatherStatusResponse>>> ByLocation(string city, string stateCode = "", string countryCode = "")
		{
			_logger.LogInformation($"{nameof(WeatherStatusController)}. Start request {nameof(ByLocation)}");

			var geocodingRequest = new GeocodingRequest(city, stateCode, countryCode, Limit: 1);
			var validationResult = _geoValidator.Validate(geocodingRequest);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var geocodingReponse = await _geoService.GetGeocodingCoordinatesByLocationAsync(geocodingRequest);

			var statusRequest = new WeatherStatusRequest(geocodingReponse.Lat, geocodingReponse.Lon);
			var result = await _statusService.GetWeatherStatusAsync(statusRequest);

			_logger.LogInformation($"{nameof(WeatherStatusController)}. Finish request {nameof(ByLocation)}");
			return TypedResults.Ok(result);
		}
	}
}
