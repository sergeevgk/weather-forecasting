﻿namespace WeatherForecasting.WebApi.Models.Response;

using System.Text.Json.Serialization;

public class SysInformation
{
	[JsonPropertyName("type")]
	public int Type { get; set; }

	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("country")]
	public string Country { get; set; }

	[JsonPropertyName("sunrise")]
	public long Sunrise { get; set; }

	[JsonPropertyName("sunset")]
	public long Sunset { get; set; }
}
