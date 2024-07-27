namespace WeatherForecasting.WebApi.Models.Contract.Response;

using System.Text.Json.Serialization;

public class Place
{
	[JsonPropertyName("city")]
	public string City { get; set; }

	[JsonPropertyName("countryCode")]
	public string CountryCode { get; set; }

	[JsonPropertyName("lon")]
	public decimal Longitude { get; set; }

	[JsonPropertyName("lat")]
	public decimal Latitude { get; set; }
}
