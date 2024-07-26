using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private const int FORECAST_RESPONSE_ITEMS_LIMIT = 40;
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;
		private readonly IValidator<BaseWeatherRequest> _validator;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IValidator<BaseWeatherRequest> validator)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_client = httpClientFactory?.CreateClient("WeatherForecastClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_settings = settings.Value;
			_validator = validator;
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherForecastByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<WeatherForecastResponse>>> ByCoordinates(decimal latitude, decimal longitude, int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation("Requesting forecast");

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

			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), limit, _settings.ApiKey);

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherForecastResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<WeatherForecastResponse> ByLocation(string city, string state = "", string countryCode = "", int limit = FORECAST_RESPONSE_ITEMS_LIMIT)
		{
			_logger.LogInformation("Requesting forecast");

			if (limit < 1 || limit > FORECAST_RESPONSE_ITEMS_LIMIT)
			{
				limit = Math.Clamp(limit, 1, FORECAST_RESPONSE_ITEMS_LIMIT);
				_logger.LogInformation($"Request limit was adjusted to {limit}.");
			}

			var request = new GeoRequest(city, state, countryCode);
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), limit, _settings.ApiKey);

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherForecastResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result;
		}

		private string BuildRequestQueryString(Dictionary<string, string> queryParameters, int forecastsLimit, string apiKey)
		{
			queryParameters.Add("appid", _settings.ApiKey);
			queryParameters.Add("cnt", forecastsLimit.ToString());
			var result = QueryHelpers.AddQueryString("", queryParameters);

			return result;
		}
	}
}
