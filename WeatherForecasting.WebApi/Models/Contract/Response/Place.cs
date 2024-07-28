using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;


public class Place
{
	[JsonProperty("city")]
	public string City { get; set; }

	[JsonProperty("countryCode")]
	public string CountryCode { get; set; }

	[JsonProperty("lon")]
	public decimal Longitude { get; set; }

	[JsonProperty("lat")]
	public decimal Latitude { get; set; }
}
