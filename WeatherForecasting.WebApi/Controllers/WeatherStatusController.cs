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
	public class WeatherStatusController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;
		private readonly IValidator<BaseWeatherRequest> _validator;

		public WeatherStatusController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IValidator<BaseWeatherRequest> validator)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_client = httpClientFactory?.CreateClient("WeatherStatusClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_settings = settings.Value;
			_validator = validator;
		}

		[ProducesResponseType(typeof(CurrentWeatherResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet("ByCoordinates", Name = "GetWeatherStatusByCoordinates")]
		public async Task<Results<BadRequest<string>, Ok<CurrentWeatherResponse>>> ByCoordinates(decimal latitude, decimal longitude)
		{
			_logger.LogInformation("Requesting current weather");

			var request = new WeatherStatusRequest(latitude, longitude);
			var validationResult = _validator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorMessageAggregated = string.Join(",", validationResult.Errors);
				var badResponseResult = TypedResults.BadRequest($"The provided parameters are invalid: {errorMessageAggregated}");

				return badResponseResult;
			}

			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), _settings.ApiKey);
			var response = await _client.GetAsync(queryString);

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<CurrentWeatherResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return TypedResults.Ok(result);
		}

		[ProducesResponseType(typeof(CurrentWeatherResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[HttpGet(Name = "GetWeatherStatus")]
		public async Task<CurrentWeatherResponse> ByLocation(string city, string state = "", string countryCode = "")
		{
			_logger.LogInformation("Requesting current weather");
			var request = new GeoRequest(city, state, countryCode);
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), _settings.ApiKey);

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<CurrentWeatherResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result;
		}

		private string BuildRequestQueryString(Dictionary<string, string> queryParameters, string apiKey)
		{
			queryParameters.Add("appid", _settings.ApiKey);
			var result = QueryHelpers.AddQueryString("", queryParameters);

			return result;
		}
	}
}
