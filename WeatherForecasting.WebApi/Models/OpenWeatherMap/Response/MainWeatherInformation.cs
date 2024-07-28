﻿using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class MainWeatherInformation
{
	[JsonProperty("temp")]
	public double Temp { get; set; }

	[JsonProperty("feels_like")]
	public double FeelsLike { get; set; }

	[JsonProperty("temp_min")]
	public double TempMin { get; set; }

	[JsonProperty("temp_max")]
	public double TempMax { get; set; }

	[JsonProperty("pressure")]
	public int Pressure { get; set; }

	[JsonProperty("humidity")]
	public int Humidity { get; set; }

	[JsonProperty("sea_level")]
	public int SeaLevel { get; set; }

	[JsonProperty("grnd_level")]
	public int GrndLevel { get; set; }
}
