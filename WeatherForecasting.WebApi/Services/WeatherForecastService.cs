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
		private readonly ITimeZoneService _timeZoneService;

		public WeatherForecastService(ILogger<WeatherForecastService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IMapper mapper, ITimeZoneService timeZoneService)
		{
			_client = httpClientFactory?.CreateClient("WeatherForecastClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_timeZoneService = timeZoneService;
		}

		public async Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequest request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var forecastReponse = JsonConvert.DeserializeObject<Models.OpenWeatherMap.Response.WeatherForecastResponse>(content);

			var result = _mapper.Map<Models.Contract.Response.WeatherForecastResponse>(forecastReponse);
			foreach (var forecast in result.Forecasts)
			{
				forecast.LocalTime = _timeZoneService.GetLocalDateTimeByCoordinates(forecast.UtcTime, result.Place.Latitude, result.Place.Longitude);
			}

			return result;
		}

		public async Task<Models.Contract.Response.WeatherForecastResponse> GetWeatherForecastAsync(WeatherForecastRequestByDate request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var forecastReponse = JsonConvert.DeserializeObject<Models.OpenWeatherMap.Response.WeatherForecastResponse>(content);

			if (request.LocalDateTime != DateTime.MinValue)
			{
				forecastReponse.List = forecastReponse.List
					.Where(forecastItem => forecastItem.ForecastDateTime >= request.UtcDateTime && forecastItem.ForecastDateTime < request.UtcDateTime.AddDays(1)).ToList();
			}
			var result = _mapper.Map<Models.Contract.Response.WeatherForecastResponse>(forecastReponse);

			foreach (var forecast in result.Forecasts)
			{
				forecast.LocalTime = _timeZoneService.GetLocalDateTimeByCoordinates(forecast.UtcTime, result.Place.Latitude, result.Place.Longitude);
			}
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
