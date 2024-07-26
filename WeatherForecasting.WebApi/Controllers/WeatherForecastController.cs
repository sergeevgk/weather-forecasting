using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_client = httpClientFactory?.CreateClient("WeatherForecastClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_settings = settings.Value;
		}

		[HttpGet("ByCoordinates", Name = "GetWeatherForecastByCoordinates")]
		public async Task<WeatherForecastResponse> ByCoordinates(decimal latitude, decimal longitude, int limit = 40)
		{
			_logger.LogInformation("Requesting forecast");
			var request = new WeatherForecastRequest(latitude, longitude, limit);
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), limit, _settings.ApiKey);

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherForecastResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<WeatherForecastResponse> ByLocation(string city, string state = "", string countryCode = "", int limit = 40)
		{
			_logger.LogInformation("Requesting forecast");
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
