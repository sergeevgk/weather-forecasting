using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherForecasting.WebApi.Models.Requests;
using WeatherForecasting.WebApi.Models.Response;

namespace WeatherForecasting.WebApi.Services
{
	public class WeatherStatusService : IWeatherStatusService
	{
		private readonly JsonSerializerOptions _serializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		private readonly ILogger<WeatherStatusService> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;

		public WeatherStatusService(ILogger<WeatherStatusService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings)
		{
			_client = httpClientFactory?.CreateClient("WeatherStatusClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger;
			_settings = settings.Value;
		}

		public async Task<WeatherStatusResponse> GetWeatherStatusAsync(WeatherStatusRequest request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WeatherStatusResponse>(content, _serializationOptions);

			return result;
		}
		private string BuildRequestQueryString(Dictionary<string, string> queryParameters)
		{
			queryParameters.Add("appid", _settings.ApiKey);
			var result = QueryHelpers.AddQueryString("", queryParameters);

			return result;
		}
	}
}
