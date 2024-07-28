using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;

namespace WeatherForecasting.WebApi.Services
{
	public class WeatherStatusService : IWeatherStatusService
	{
		private readonly ILogger<WeatherStatusService> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;
		private readonly IMapper _mapper;
		private readonly ITimeZoneService _timeZoneService;

		public WeatherStatusService(ILogger<WeatherStatusService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IMapper mapper, ITimeZoneService timeZoneService)
		{
			_client = httpClientFactory?.CreateClient("WeatherStatusClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_timeZoneService = timeZoneService;
		}

		public async Task<Models.Contract.Response.WeatherStatusResponse> GetWeatherStatusAsync(WeatherStatusRequest request)
		{
			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var weatherStatusReponse = JsonConvert.DeserializeObject<Models.OpenWeatherMap.Response.WeatherStatusResponse>(content);

			var result = _mapper.Map<Models.Contract.Response.WeatherStatusResponse>(weatherStatusReponse);
			result.StatusLocalTime = _timeZoneService.GetLocalDateTimeByCoordinates(result.StatusUtcTime, result.Place.Latitude, result.Place.Longitude);

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
