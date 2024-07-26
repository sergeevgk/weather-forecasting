using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public class WeatherForecastService : IWeatherForecastService
	{
		private readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		private readonly ILogger<WeatherForecastService> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;

		public WeatherForecastService(ILogger<WeatherForecastService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings)
		{
			_client = httpClientFactory?.CreateClient("WeatherForecastClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger;
			_settings = settings.Value;
		}

		public async Task<WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary(), request.ForecastsCount);

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherForecastResponse>(content, _serializationOptions);

			return result;
		}

		private string BuildRequestQueryString(Dictionary<string, string> queryParameters, int forecastsLimit)
		{
			queryParameters.Add("appid", _settings.ApiKey);
			queryParameters.Add("cnt", forecastsLimit.ToString());
			var result = QueryHelpers.AddQueryString("", queryParameters);

			return result;
		}
	}
}
