namespace WeatherForecasting.WebApi.Mapping;

using AutoMapper;
using Newtonsoft.Json;
using System;
using WeatherForecasting.WebApi.Helpers;
using WeatherForecasting.WebApi.Models.Contract.Response;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;
using WeatherForecasting.WebApi.Services;

public class WeatherMappingProfile : Profile
{
	public WeatherMappingProfile()
	{
		CreateMap<Coordinates, Place>()
			.ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Lon))
			.ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Lat))
			.ForMember(dest => dest.City, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.CountryCode, opt => opt.Ignore()); // These will be mapped separately

		CreateMap<Weather, WeatherSummary>()
			.ForMember(dest => dest.Main, opt => opt.MapFrom(src => src.Main))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.Temperature, opt => opt.Ignore()) // Temperature will be mapped separately
			.ForMember(dest => dest.Pressure, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.Humidity, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.CloudyPercentage, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.RainVolume1Hour, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.RainVolume3Hour, opt => opt.Ignore());  // These will be mapped separately

		CreateMap<MainWeatherInformation, Temperature>()
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Temp))
			.ForMember(dest => dest.FeelsLike, opt => opt.MapFrom(src => src.FeelsLike));

		CreateMap<MainWeatherInformation, WeatherSummary>()
			.ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src))
			.ForMember(dest => dest.Pressure, opt => opt.MapFrom(src => src.Pressure))
			.ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Humidity))
			.ForMember(dest => dest.CloudyPercentage, opt => opt.Ignore()); // These will be mapped separately

		CreateMap<Clouds, WeatherSummary>()
			.ForMember(dest => dest.CloudyPercentage, opt => opt.MapFrom(src => src.All));
		CreateMap<Rain, WeatherSummary>()
			.ForMember(dest => dest.RainVolume1Hour, opt => opt.MapFrom(src => src.VolumeOneHour))
			.ForMember(dest => dest.RainVolume3Hour, opt => opt.MapFrom(src => src.VolumeThreeHour));
		CreateMap<Snow, WeatherSummary>()
			.ForMember(dest => dest.SnowVolume1Hour, opt => opt.MapFrom(src => src.VolumeOneHour))
			.ForMember(dest => dest.SnowVolume3Hour, opt => opt.MapFrom(src => src.VolumeThreeHour));

		CreateMap<Models.OpenWeatherMap.Response.Wind, Models.Contract.Response.Wind>();

		CreateMap<Models.OpenWeatherMap.Response.WeatherStatusResponse, Models.Contract.Response.WeatherStatusResponse>()
			.ForMember(dest => dest.StatusLocalTime, opt => opt.MapFrom(src => MapUtcTimestampToLocalTime(src.UnixTimestampUtc, src.Coord.Lat, src.Coord.Lon)))
			.ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.Coord))
			.ForMember(dest => dest.WeatherSummary, opt => opt.MapFrom(src => src.MainInfo))
			.ForMember(dest => dest.Wind, opt => opt.MapFrom(src => src.Wind))
			.AfterMap((src, dest) =>
			{
				if (src.Weather != null && src.Weather.Count > 0)
				{
					var weather = src.Weather[0];
					dest.WeatherSummary.Main = weather.Main;
					dest.WeatherSummary.Description = weather.Description;
				}
				dest.Place.City = src.Name;
				dest.Place.CountryCode = src.Sys.Country;

				if (src.Clouds != null)
				{
					dest.WeatherSummary.CloudyPercentage = src.Clouds.All;
				}
			});

		CreateMap<Models.OpenWeatherMap.Response.WeatherForecastItem, Models.Contract.Response.WeatherForecastItem>()
			.ForMember(dest => dest.LocalTime, opt => opt.Ignore()) // These will be mapped separately
			.ForMember(dest => dest.WeatherSummary, opt => opt.MapFrom(src => src.Main))
			.ForMember(dest => dest.Wind, opt => opt.MapFrom(src => src.Wind))
			.AfterMap((src, dest) =>
			{
				if (src.Weather != null && src.Weather.Count > 0)
				{
					var weather = src.Weather[0];
					dest.WeatherSummary.Main = weather.Main;
					dest.WeatherSummary.Description = weather.Description;
				}
			
				if (src.Clouds != null)
				{
					dest.WeatherSummary.CloudyPercentage = src.Clouds.All;
				}
			});

		CreateMap<Models.OpenWeatherMap.Response.WeatherForecastResponse, Models.Contract.Response.WeatherForecastResponse>()
			.ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.City.Coordinates))
			.ForMember(dest => dest.Forecasts, opt => opt.MapFrom(src => src.List))
			.AfterMap((src, dest) =>
			{
				dest.Place.City = src.City.Name;
				dest.Place.CountryCode = src.City.Country;

				for (int i = 0; i < src.List.Count; i++)
				{
					dest.Forecasts[i].LocalTime = TimeZoneService.GetLocalDateTimeByCoordinates(src.List[i].ForecastDateTime, src.City.Coordinates.Lat, src.City.Coordinates.Lon);
				}
			});
	}

	private static DateTime MapUtcTimestampToLocalTime(long unixTimestampUtc, decimal lat, decimal lon)
	{
		var utcTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestampUtc).DateTime;
		var localTime = TimeZoneService.GetLocalDateTimeByCoordinates(utcTime, lat, lon);

		return localTime;
	}
}