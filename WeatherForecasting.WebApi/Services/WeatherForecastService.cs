using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

namespace WeatherForecasting.WebApi.Services
{
	public class WeatherForecastService : IWeatherForecastService
	{
		private readonly ILogger<WeatherForecastService> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;
		private readonly IMapper _mapper;

		public WeatherForecastService(ILogger<WeatherForecastService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IMapper mapper)
		{
			_client = httpClientFactory?.CreateClient("WeatherForecastClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var forecastReponse = JsonConvert.DeserializeObject<Models.OpenWeatherMap.Response.WeatherForecastResponse>(content);

			var result = _mapper.Map<Models.Contract.Response.WeatherForecastResponse>(forecastReponse);

			return result;
		}

		public async Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequestByDate request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var forecastReponse = JsonConvert.DeserializeObject<Models.OpenWeatherMap.Response.WeatherForecastResponse>(content);

			if (request.Date != DateOnly.MinValue)
			{
				forecastReponse.List = forecastReponse.List.Where(forecastItem => forecastItem.ForecastDateTime.Date == request.UtcDateTime.Date).ToList();
			}
			var result = _mapper.Map<Models.Contract.Response.WeatherForecastResponse>(forecastReponse);

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
