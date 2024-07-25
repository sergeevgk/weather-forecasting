using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.Json;
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

		[HttpGet(Name = "GetCurrentWeather")]
		public async Task<WeatherResponse> GetCurrentWeather()
		{
			var query = BuildRequestQuery(10, 10, _settings.ApiKey);
			var response = await _client.GetAsync(query);
			
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return result;
		}

		private string BuildRequestQuery(decimal latitude, decimal longitude, string apiKey)
		{
			var queryParameterDictionary = new Dictionary<string, string>()
			{
				{ "lat", latitude.ToString("N2") },
				{ "lon", longitude.ToString("N2") },
				{ "appid", apiKey }

			};
			var result = QueryHelpers.AddQueryString("", queryParameterDictionary);
			return result;
		}
	}
}
