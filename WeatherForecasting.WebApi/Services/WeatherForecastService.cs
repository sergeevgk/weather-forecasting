using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

namespace WeatherForecasting.WebApi.Services
{
	public class WeatherForecastService : IWeatherForecastService
	{
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
			var forecastReponse = JsonConvert.DeserializeObject<WeatherForecastResponse>(content);

			if (request.UseDateFilter)
			{
				forecastReponse.List = forecastReponse.List.Where(forecastItem => forecastItem.ForecastDateTime.Date == request.UtcDateTime.Date).ToList();
			}

			var result = forecastReponse;
			return result;
		}

		private string BuildRequestQueryString(Dictionary<string, string> queryParameters, int forecastsLimit)
		{
			queryParameters.Add("appid", _settings.ApiKey);
			var result = QueryHelpers.AddQueryString("", queryParameters);

			return result;
		}
	}
}
