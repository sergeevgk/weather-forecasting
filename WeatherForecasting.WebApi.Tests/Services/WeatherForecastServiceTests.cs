using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Mapping;
using WeatherForecasting.WebApi.Models.Contract.Response;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;
using WeatherForecasting.WebApi.Services;
using WeatherForecastItem = WeatherForecasting.WebApi.Models.OpenWeatherMap.Response.WeatherForecastItem;
using WeatherForecastResponse = WeatherForecasting.WebApi.Models.OpenWeatherMap.Response.WeatherForecastResponse;

namespace WeatherForecasting.WebApi.Tests.Services
{
	public class WeatherForecastServiceTests
	{
		private const string BASE_URI = "https://test";
		private Mock<FakeHttpMessageHandler> _mockHttpMessageHandler;
		private Mock<IHttpClientFactory> _mockHttpCLientFactory;
		private Mock<ILogger<WeatherForecastService>> _mockLogger;
		private Mock<IOptions<OpenWeatherMapSettings>> _mockSettings;
		private Mock<IMapper> _mockMapper;
		private Mock<ITimeZoneService> _mockTimeZoneService;

		[SetUp]
		public void Setup()
		{
			_mockHttpMessageHandler = new Mock<FakeHttpMessageHandler>() { CallBase = true };
			var httpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri(BASE_URI) };
			_mockHttpCLientFactory = new Mock<IHttpClientFactory>();
			_mockHttpCLientFactory.Setup(f => f.CreateClient("WeatherForecastClient")).Returns(httpClient);
			_mockSettings = new Mock<IOptions<OpenWeatherMapSettings>>();
			_mockSettings.Setup(s => s.Value)
				.Returns(new OpenWeatherMapSettings
				{
					ApiKey = "test",
					BaseUri = ""
				});
			_mockLogger = new Mock<ILogger<WeatherForecastService>>();
			_mockMapper = new Mock<IMapper>();
			_mockTimeZoneService = new Mock<ITimeZoneService>();
			_mockTimeZoneService.Setup(s => s.GetLocalDateTimeByCoordinates(It.IsAny<DateTime>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
				.Returns((DateTime time, decimal lat, decimal lon) => time);
		}

		[TestCase("2024-08-01", 3, TestName = "1 - Returns weather status response with forecasts filtered by date.")]
		[TestCase("2024-08-02", 1, TestName = "2 - Returns weather status response with forecasts filtered by date.")]
		[TestCase("2024-08-03", 2, TestName = "3 - Returns weather status response with forecasts filtered by date.")]
		[TestCase("2024-08-04", 0, TestName = "4 - Returns weather status response with forecasts filtered by date.")]
		public async Task GetWeatherForecastAsync_ByDate_ReturnsCorrectResponse(string requestDateString, int expectedForecastCount)
		{
			// Arrange
			var requestDate = DateTime.Parse(requestDateString, null, System.Globalization.DateTimeStyles.RoundtripKind);
			var utcRequestDateTime = requestDate;
			var request = new WeatherForecastRequestByDate(10, 10, requestDate, utcRequestDateTime);

			var expectedReponse = new Models.Contract.Response.WeatherForecastResponse
			{
				Place = new Place { City = "test" },
				Forecasts = [
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-01"),
						UtcTime = DateTime.Parse("2024-08-01"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 1, Value = 0 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-01"),
						UtcTime = DateTime.Parse("2024-08-01"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 20, Value = 19 },
							Description = "warm"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-02"),
						UtcTime = DateTime.Parse("2024-08-02"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 3, Value = 2 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-03"),
						UtcTime = DateTime.Parse("2024-08-03"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 4, Value = 1 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-03"),
						UtcTime = DateTime.Parse("2024-08-03"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 0, Value = 0 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-01"),
						UtcTime = DateTime.Parse("2024-08-01"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 0, Value = 0 },
							Description = "cold"
						}
					},
				]
				
			};
			var expectedForecastsFilteredByDate = expectedReponse.Forecasts
				.Where(i => i.LocalTime.Date == requestDate).ToArray();

			var externalServiceResponse = new WeatherForecastResponse
			{
				City = new City { Name = "test" },
				List = [
					new WeatherForecastItem 
					{
						ForecastDateTime = DateTime.Parse("2024-08-01"),
						Main = new MainWeatherInformation {Temp = 0, FeelsLike = 1},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-01"),
						Main = new MainWeatherInformation {Temp = 19, FeelsLike = 20},
						Weather = [new Weather { Description = "warm" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-02"),
						Main = new MainWeatherInformation {Temp = 2, FeelsLike = 3},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-03"),
						Main = new MainWeatherInformation {Temp = 1, FeelsLike = 4},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-03"),
						Main = new MainWeatherInformation {Temp = 0, FeelsLike = 0},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-01"),
						Main = new MainWeatherInformation {Temp = 0, FeelsLike = 0},
						Weather = [new Weather { Description = "cold" }]
					}
				]
			};

			var serializedExpectedResponse = JsonConvert.SerializeObject(externalServiceResponse);
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(serializedExpectedResponse)
				});

			var mapperCongig = new MapperConfiguration(cfg => cfg.AddMaps(new[] { typeof(WeatherMappingProfile) }));
			var service = new WeatherForecastService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, new Mapper(mapperCongig), _mockTimeZoneService.Object);

			//Act
			var result = await service.GetWeatherForecastAsync(request);

			//Assert
			Assert.NotNull(result);
			Assert.AreEqual(expectedForecastCount, result.Forecasts.Count());
			Assert.AreEqual(expectedReponse.Place.City, result.Place.City);
			for (int i = 0; i < expectedForecastCount; i++)
			{
				var expectedItem = expectedForecastsFilteredByDate[i];
				var resultItem = result.Forecasts[i];
				Assert.AreEqual(expectedItem.LocalTime, resultItem.LocalTime);
				Assert.AreEqual(expectedItem.UtcTime, resultItem.UtcTime);
				Assert.AreEqual(expectedItem.WeatherSummary.Temperature, resultItem.WeatherSummary.Temperature);
				Assert.AreEqual(expectedItem.WeatherSummary.Description, resultItem.WeatherSummary.Description);
			}
		}

		[TestCase(4, 1, 1, TestName = "5 - Returns 1 foreacst, limited by request limit = 1.")]
		[TestCase(4, 3, 3, TestName = "6 - Returns 3 forecasts, limited by request limit = 3.")]
		[TestCase(4, 4, 4, TestName = "7 - Returns 4 forecasts limited by request limit = 4 and and external service forecasts count = 4.")]
		[TestCase(4, 5, 4, TestName = "8 - Returns 4 forecasts limited by request limit = 5 and external service forecasts count = 4.")]
		[TestCase(4, 40, 4, TestName = "9 - Returns 4 forecasts number limited by request limit = 40 and external service forecasts count = 4.")]
		public async Task GetWeatherForecastAsync_ByLimit_ReturnsCorrectResponse(int totalForecastsInExternalServiceReponse, int limit, int expectedForecastsCount)
		{
			// Arrange
			var expectedReponse = new Models.Contract.Response.WeatherForecastResponse
			{
				Place = new Place { City = "test" },
				Forecasts = [
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-01"),
						UtcTime = DateTime.Parse("2024-08-01"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 1, Value = 0 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-01"),
						UtcTime = DateTime.Parse("2024-08-01"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 20, Value = 19 },
							Description = "warm"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-02"),
						UtcTime = DateTime.Parse("2024-08-02"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 3, Value = 2 },
							Description = "cold"
						}
					},
					new Models.Contract.Response.WeatherForecastItem
					{
						LocalTime = DateTime.Parse("2024-08-03"),
						UtcTime = DateTime.Parse("2024-08-03"),
						WeatherSummary = new WeatherSummary
						{
							Temperature = new Temperature { FeelsLike = 4, Value = 1 },
							Description = "cold"
						}
					}
				]

			};
			var expectedForecastsLimited = expectedReponse.Forecasts
				.Take(limit).ToArray();

			// todo: create totalForecastsInExternalServiceReponse items
			var externalServiceResponse = new WeatherForecastResponse
			{
				City = new City { Name = "test" },
				List = [
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-01"),
						Main = new MainWeatherInformation {Temp = 0, FeelsLike = 1},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-01"),
						Main = new MainWeatherInformation {Temp = 19, FeelsLike = 20},
						Weather = [new Weather { Description = "warm" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-02"),
						Main = new MainWeatherInformation {Temp = 2, FeelsLike = 3},
						Weather = [new Weather { Description = "cold" }]
					},
					new WeatherForecastItem
					{
						ForecastDateTime = DateTime.Parse("2024-08-03"),
						Main = new MainWeatherInformation {Temp = 1, FeelsLike = 4},
						Weather = [new Weather { Description = "cold" }]
					}
				]
			};
			externalServiceResponse.List = externalServiceResponse.List.Take(limit).ToList();

			var serializedExpectedResponse = JsonConvert.SerializeObject(externalServiceResponse);
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(serializedExpectedResponse)
				});

			var mapperCongig = new MapperConfiguration(cfg => cfg.AddMaps(new[] { typeof(WeatherMappingProfile) }));
			var service = new WeatherForecastService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, new Mapper(mapperCongig), _mockTimeZoneService.Object);
			var request = new WeatherForecastRequest(0, 0, limit);

			//Act
			var result = await service.GetWeatherForecastAsync(request);

			//Assert
			Assert.NotNull(result);
			Assert.AreEqual(expectedForecastsCount, result.Forecasts.Count());
			Assert.AreEqual(expectedReponse.Place.City, result.Place.City);
			for (int i = 0; i < expectedForecastsCount; i++)
			{
				var expectedItem = expectedForecastsLimited[i];
				var resultItem = result.Forecasts[i];
				Assert.AreEqual(expectedItem.LocalTime, resultItem.LocalTime);
				Assert.AreEqual(expectedItem.UtcTime, resultItem.UtcTime);
				Assert.AreEqual(expectedItem.WeatherSummary.Temperature, resultItem.WeatherSummary.Temperature);
				Assert.AreEqual(expectedItem.WeatherSummary.Description, resultItem.WeatherSummary.Description);
			}
		}


		internal record ChangedForecastResponse(string Name, string Temperature, double Latitude, double Longitude, string Time) { }

		[TestCase(TestName = "Throw an exception when extrenal service returns a different model")]
		public async Task GetWeatherForecastAsync_ThrowException_WhenExternalContractChanges()
		{
			// Arrange
			var serializedExpectedResponse = JsonConvert.SerializeObject(new ChangedForecastResponse("test", "te", 10.0d, 10.0d, "0000-00-00 00:00:00"));
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(serializedExpectedResponse)
				});

			_mockMapper
				.Setup(m => m.Map<Models.Contract.Response.WeatherForecastResponse>(It.IsAny<WeatherForecastResponse>()))
				.Throws(new JsonSerializationException("Error mapping WeatherForecastResponse."));

			var service = new WeatherForecastService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, _mockMapper.Object, _mockTimeZoneService.Object);
			var request = new WeatherForecastRequest(0, 0, 1);

			//Act, Assert
			var ex = Assert.ThrowsAsync<JsonSerializationException>(async () => await service.GetWeatherForecastAsync(request));
		}

		[TestCase(TestName = "Throw an exception when extrenal service does not respond")]
		public void GetWeatherForecastAsync_ThrowException_WhenExternalServiceUnavailable()
		{
			// Arrange
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Throws(() => new TimeoutException("External service did not repond."));

			var service = new WeatherForecastService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, _mockMapper.Object, _mockTimeZoneService.Object);
			var request = new WeatherForecastRequest(0, 0, 1);

			//Act, Assert
			var ex = Assert.ThrowsAsync<TimeoutException>(async () => await service.GetWeatherForecastAsync(request));
		}
	}
}
