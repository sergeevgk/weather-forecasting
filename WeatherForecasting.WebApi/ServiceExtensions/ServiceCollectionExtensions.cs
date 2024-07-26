using Microsoft.Extensions.Options;

namespace WeatherForecasting.WebApi.ServiceExtensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddWeatherForecastingHttpClient(this IServiceCollection services)
		{
			services.AddHttpClient("WeatherForecastClient", (serviceProvider, client) =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<OpenWeatherMapSettings>>().Value;
				var baseUri = new Uri(settings.BaseUri);
				client.BaseAddress = new Uri(baseUri, relativeUri: "data/2.5/forecast");
			});
		}

		public static void AddWeatherStatusHttpClient(this IServiceCollection services)
		{
			services.AddHttpClient("WeatherStatusClient", (serviceProvider, client) =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<OpenWeatherMapSettings>>().Value;
				var baseUri = new Uri(settings.BaseUri);
				client.BaseAddress = new Uri(baseUri, relativeUri: "data/2.5/weather");
			});
		}

		public static void AddGeocodingHttpClient(this IServiceCollection services)
		{
			services.AddHttpClient("GeocodingClient", (serviceProvider, client) =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<OpenWeatherMapSettings>>().Value;
				var baseUri = new Uri(settings.BaseUri);
				client.BaseAddress = new Uri(baseUri, relativeUri: "geo/1.0/direct");
			});
		}
	}
}
