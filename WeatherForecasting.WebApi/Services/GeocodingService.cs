using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

namespace WeatherForecasting.WebApi.Services
{
	public class GeocodingService : IGeocodingService
	{
		private readonly ILogger<GeocodingService> _logger;
		private readonly HttpClient _client;
		private readonly OpenWeatherMapSettings _settings;
		private readonly IDistributedCache _cache;

		public GeocodingService(ILogger<GeocodingService> logger, IHttpClientFactory httpClientFactory, IOptions<OpenWeatherMapSettings> settings, IDistributedCache cache)
		{
			_client = httpClientFactory?.CreateClient("GeocodingClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public async Task<GeocodingResponse> GetGeocodingCoordinatesByLocationAsync(GeocodingRequest request)
		{
			var cacheKey = $"Geo:{request.CityName}:{request.StateCode}:{request.CountryCode}";
			var cachedGeoResponse = await _cache.GetAsync(cacheKey);
			GeocodingResponse result;
			if (cachedGeoResponse != null)
			{
				result = JsonSerializer.Deserialize<GeocodingResponse>(cachedGeoResponse);
				return result;
			}

			var queryString = BuildRequestQueryString(request.ToQueryParametersDictionary());

			var response = await _client.GetAsync(queryString);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var resultArray = Newtonsoft.Json.JsonConvert.DeserializeObject<GeocodingResponse[]>(content);
			result = resultArray.FirstOrDefault();

			var cacheEntryOptions = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
			};
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheEntryOptions);

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
