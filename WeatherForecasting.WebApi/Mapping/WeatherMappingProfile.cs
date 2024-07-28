using AutoMapper;
using WeatherForecasting.WebApi.Models.Contract.Response;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Mapping;

public class WeatherMappingProfile : Profile
{
	public WeatherMappingProfile()
	{
		CreateMap<Coordinates, Place>()
			.ForMember(x => x.Latitude, opt => opt.MapFrom(src => src.Lat))
			.ForMember(x => x.Longitude, opt => opt.MapFrom(src => src.Lon));

		CreateMap<SysInformation, Place>()
			.ForMember(x => x.CountryCode, opt => opt.MapFrom(src => src.Country));

		CreateMap<City, Place>()
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country))
			.IncludeMembers(s => s.Coordinates);

		CreateMap<Models.OpenWeatherMap.Response.WeatherStatusResponse, Place>()
			.ForMember(x => x.City, opt => opt.MapFrom(src => src.Name))
			.IncludeMembers(s => s.Sys, s => s.Coord);

		CreateMap<Weather, WeatherSummary>()
			.ForMember(dest => dest.Main, opt => opt.MapFrom(src => src.Main))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

		CreateMap<MainWeatherInformation, Temperature>()
			.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Temp))
			.ForMember(dest => dest.FeelsLike, opt => opt.MapFrom(src => src.FeelsLike));

		CreateMap<MainWeatherInformation, WeatherSummary>()
			.ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src))
			.ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Humidity))
			.ForMember(dest => dest.Pressure, opt => opt.MapFrom(src => src.Pressure));

		CreateMap<Clouds, WeatherSummary>()
			.ForMember(dest => dest.CloudyPercentage, opt => opt.MapFrom(src => src.All));
		CreateMap<Rain, WeatherSummary>()
			.ForMember(dest => dest.RainVolume1Hour, opt => opt.MapFrom(src => src.VolumeOneHour))
			.ForMember(dest => dest.RainVolume3Hour, opt => opt.MapFrom(src => src.VolumeThreeHour));
		CreateMap<Snow, WeatherSummary>()
			.ForMember(dest => dest.SnowVolume1Hour, opt => opt.MapFrom(src => src.VolumeOneHour))
			.ForMember(dest => dest.SnowVolume3Hour, opt => opt.MapFrom(src => src.VolumeThreeHour));

		CreateMap<Models.OpenWeatherMap.Response.WeatherStatusResponse, WeatherSummary>()
			.IncludeMembers(s => s.MainInfo, s => s.Clouds, s => s.Rain, s => s.Snow);

		CreateMap<Models.OpenWeatherMap.Response.Wind, Models.Contract.Response.Wind>();

		CreateMap<Models.OpenWeatherMap.Response.WeatherStatusResponse, Models.Contract.Response.WeatherStatusResponse>()
			.ForMember(dest => dest.StatusUtcTime, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.UnixTimestampUtc).DateTime))
			.ForMember(dest => dest.Place, opt => opt.MapFrom(src => src))
			.ForMember(dest => dest.WeatherSummary, opt => opt.MapFrom(src => src))
			.ForMember(dest => dest.Wind, opt => opt.MapFrom(src => src.Wind))
			.AfterMap((src, dest) =>
			{
				if (src.Weather != null && src.Weather.Count > 0)
				{
					var weather = src.Weather[0];
					dest.WeatherSummary.Main = weather.Main;
					dest.WeatherSummary.Description = weather.Description;
				}
			});

		CreateMap<Models.OpenWeatherMap.Response.WeatherForecastItem, WeatherSummary>()
			.IncludeMembers(s => s.Main, s => s.Clouds, s => s.Rain, s => s.Snow);

		CreateMap<Models.OpenWeatherMap.Response.WeatherForecastItem, Models.Contract.Response.WeatherForecastItem>()
			.ForMember(dest => dest.WeatherSummary, opt => opt.MapFrom(src => src))
			.ForMember(dest => dest.Wind, opt => opt.MapFrom(src => src.Wind))
			.ForMember(dest => dest.UtcTime, opt => opt.MapFrom(src => src.ForecastDateTime))
			.AfterMap((src, dest) =>
			{
				if (src.Weather != null && src.Weather.Count > 0)
				{
					var weather = src.Weather[0];
					dest.WeatherSummary.Main = weather.Main;
					dest.WeatherSummary.Description = weather.Description;
				}
			});

		CreateMap<Models.OpenWeatherMap.Response.WeatherForecastResponse, Models.Contract.Response.WeatherForecastResponse>()
			.ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.City))
			.ForMember(dest => dest.Forecasts, opt => opt.MapFrom(src => src.List));
	}
}