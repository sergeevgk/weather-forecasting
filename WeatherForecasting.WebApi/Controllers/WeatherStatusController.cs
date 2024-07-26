using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
	public class WeatherStatusController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;

		public WeatherStatusController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_client = httpClientFactory?.CreateClient("WeatherStatusClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_settings = settings.Value;
		}

		[HttpGet("ByCoordinates", Name = "GetWeatherStatusByCoordinates")]
		public async Task<CurrentWeatherResponse> ByCoordinates(decimal latitude, decimal longitude)
		{
			_logger.LogInformation("Requesting current weather");

			var request = new WeatherStatusRequest(latitude, longitude);
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), _settings.ApiKey);
			var response = await _client.GetAsync(queryString);
			
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<CurrentWeatherResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result;
		}

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
