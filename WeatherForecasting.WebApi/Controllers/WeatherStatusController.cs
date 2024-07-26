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
	public class WeatherStatusController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IValidator<BaseWeatherRequest> _validator;
		private readonly IWeatherStatusService _statusService;
		private readonly IGeocodingService _geoService;

		public WeatherStatusController(ILogger<WeatherForecastController> logger, IWeatherStatusService statusService, IGeocodingService geoService, IValidator<BaseWeatherRequest> validator)
		{
			_statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
			_geoService = geoService ?? throw new ArgumentNullException(nameof(geoService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_validator = validator;
		}

		[ProducesResponseType(typeof(WeatherStatusResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherStatusByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<WeatherStatusResponse>>> ByCoordinates(decimal latitude, decimal longitude)
		{
			_logger.LogInformation($"{nameof(WeatherStatusController)}. Start request {nameof(ByCoordinates)}");

			var request = new WeatherStatusRequest(latitude, longitude);
			var validationResult = _validator.Validate(request);
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
		public async Task<WeatherStatusResponse> ByLocation(string city, string state = "", string countryCode = "")
		{
			_logger.LogInformation($"{nameof(WeatherStatusController)}. Start request {nameof(ByLocation)}");

			var geocodingRequest = new GeocodingRequest(city, state, countryCode);
			// TODO: add validation?
			var geocodingReponse = _geoService.GetCoordinatesByLocation(geocodingRequest);

			var statusRequest = new WeatherStatusRequest(geocodingReponse.Lat, geocodingReponse.Lon);
			var result = await _statusService.GetWeatherStatusAsync(statusRequest);

			_logger.LogInformation($"{nameof(WeatherStatusController)}. Finish request {nameof(ByLocation)}");
			return result;
		}
	}
}
