using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Mapping;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Services;
using WeatherStatusResponse = WeatherForecasting.WebApi.Models.OpenWeatherMap.Response.WeatherStatusResponse;

namespace WeatherForecasting.WebApi.Tests.Services
{
	[TestFixture]
	public class WeatherStatusServiceTests
	{
		private const string BASE_URI = "https://test";
		private Mock<FakeHttpMessageHandler> _mockHttpMessageHandler;
		private Mock<IHttpClientFactory> _mockHttpCLientFactory;
		private Mock<ILogger<WeatherStatusService>> _mockLogger;
		private Mock<IOptions<OpenWeatherMapSettings>> _mockSettings;
		private Mock<IMapper> _mockMapper;
		private Mock<ITimeZoneService> _mockTimeZoneService;

		[SetUp]
		public void Setup()
		{
			_mockHttpMessageHandler = new Mock<FakeHttpMessageHandler>() { CallBase = true };
			var httpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri(BASE_URI) };
			_mockHttpCLientFactory = new Mock<IHttpClientFactory>();
			_mockHttpCLientFactory.Setup(f => f.CreateClient("WeatherStatusClient")).Returns(httpClient);
			_mockSettings = new Mock<IOptions<OpenWeatherMapSettings>>();
			_mockSettings.Setup(s => s.Value)
				.Returns(new OpenWeatherMapSettings
				{
					ApiKey = "test",
					BaseUri = ""
				});
			_mockLogger = new Mock<ILogger<WeatherStatusService>>();
			_mockMapper = new Mock<IMapper>();
			_mockTimeZoneService = new Mock<ITimeZoneService>();
			_mockTimeZoneService.Setup(s => s.GetLocalDateTimeByCoordinates(It.IsAny<DateTime>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
				.Returns((DateTime time, decimal lat, decimal lon) => time);
		}

		[TestCase(TestName = "Returns weather status response")]
		public async Task GetWeatherStatusAsync_ReturnsCorrectResponse()
		{
			// Arrange
			long unixTimestamp = 1722190910;
			var dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
			var expectedReponse = new Models.Contract.Response.WeatherStatusResponse
			{
				Place = new Models.Contract.Response.Place { City = "test" },
				StatusLocalTime = dateTimeUtc,
				StatusUtcTime = dateTimeUtc,
				WeatherSummary = new Models.Contract.Response.WeatherSummary 
				{ 
					Temperature = new Models.Contract.Response.Temperature { FeelsLike = 1, Value = 0},
					Description = "cold"
				}
			};

			var externalServiceResponse = new WeatherStatusResponse
			{
				Id = 1,
				UnixTimestampUtc = unixTimestamp,
				Name = "test",
				Weather =
				[
					new Models.OpenWeatherMap.Response.Weather { Id = 1, Description = "cold" } 
				],
				MainInfo = new Models.OpenWeatherMap.Response.MainWeatherInformation { FeelsLike = 1, Temp = 0 }
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
			var service = new WeatherStatusService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, new Mapper(mapperCongig), _mockTimeZoneService.Object);
			var request = new WeatherStatusRequest(0, 0);

			//Act
			var response = await service.GetWeatherStatusAsync(request);

			//Assert
			Assert.NotNull(response);
			Assert.AreEqual(expectedReponse.Place.City, response.Place.City);
			Assert.AreEqual(expectedReponse.WeatherSummary.Temperature, response.WeatherSummary.Temperature);
			Assert.AreEqual(expectedReponse.WeatherSummary.Description, response.WeatherSummary.Description);
			Assert.AreEqual(expectedReponse.StatusUtcTime, response.StatusUtcTime);
			Assert.AreEqual(expectedReponse.StatusLocalTime, response.StatusLocalTime);
		}

		[TestCase(TestName = "Throw an exception when extrenal service returns a different model")]
		public async Task GetWeatherStatusAsync_ThrowException_WhenExternalContractChanges()
		{
			// Arrange
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent("test")
				});

			_mockMapper
				.Setup(m => m.Map<Models.Contract.Response.WeatherStatusResponse>(It.IsAny<WeatherStatusResponse>()))
				.Throws(new JsonSerializationException("Error mapping WeatherStatusResponse."));

			var service = new WeatherStatusService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, _mockMapper.Object, _mockTimeZoneService.Object);
			var request = new WeatherStatusRequest(0, 0);

			//Act, Assert
			var ex = Assert.ThrowsAsync<ApplicationException>(async () => await service.GetWeatherStatusAsync(request));
			Assert.NotNull(ex.Message);
			Assert.IsTrue(ex.InnerException is JsonSerializationException);
			Assert.Contains(new KeyValuePair<string, string>("StatusCode", "500"), ex.Data);
		}

		[TestCase(TestName = "Throw an exception when extrenal service does not respond")]
		public void GetWeatherStatusAsync_ThrowException_WhenExternalServiceUnavailable()
		{
			// Arrange
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Throws(() => new TimeoutException("External service did not repond."));

			var service = new WeatherStatusService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object, _mockMapper.Object, _mockTimeZoneService.Object);
			var request = new WeatherStatusRequest(0, 0);

			//Act, Assert
			var ex = Assert.ThrowsAsync<ApplicationException>(async () => await service.GetWeatherStatusAsync(request));
			Assert.NotNull(ex.Message);
			Assert.IsTrue(ex.InnerException is TimeoutException);
			Assert.Contains(new KeyValuePair<string, string>("StatusCode", "502"), ex.Data);
		}
	}
}
