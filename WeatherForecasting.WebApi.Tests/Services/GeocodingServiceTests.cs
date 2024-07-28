using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Request;
using WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;
using WeatherForecasting.WebApi.Services;

namespace WeatherForecasting.WebApi.Tests.Services
{
	[TestFixture]
	public class GeocodingServiceTests
	{
		private const string BASE_URI = "https://test";
		private Mock<FakeHttpMessageHandler> _mockHttpMessageHandler;
		private Mock<IHttpClientFactory> _mockHttpCLientFactory;
		private Mock<ILogger<GeocodingService>> _mockLogger;
		private Mock<IOptions<OpenWeatherMapSettings>> _mockSettings;
		private Dictionary<string, List<GeocodingResponse>> _requestResponseDictionary;

		[SetUp]
		public void Setup()
		{
			var singleItemResponse = new List<GeocodingResponse>
			{
				new GeocodingResponse
				{
					Country = "DE",
					Name = "Test",
					LocalNames = new Dictionary<string, string> { { "de", "test" } }
				}
			};
			var severalItemResponse = new List<GeocodingResponse>
			{
				new GeocodingResponse
				{
					Country = "DE",
					Name = "Test",
					LocalNames = new Dictionary<string, string> { { "de", "test" } }
				},
				new GeocodingResponse
				{
					Country = "NL",
					Name = "Test",
					LocalNames = new Dictionary<string, string> { { "nl", "test" } }
				},
			};

			_requestResponseDictionary = new Dictionary<string, List<GeocodingResponse>>
			{
				{ "emptyResponse", new List<GeocodingResponse>() },
				{ "singleItemResponse", singleItemResponse },
				{ "severalItemResponse", severalItemResponse },
			};

			_mockHttpMessageHandler = new Mock<FakeHttpMessageHandler>() { CallBase = true };
			var httpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri(BASE_URI) };
			_mockHttpCLientFactory = new Mock<IHttpClientFactory>();
			_mockHttpCLientFactory.Setup(f => f.CreateClient("GeocodingClient")).Returns(httpClient);
			_mockSettings = new Mock<IOptions<OpenWeatherMapSettings>>();
			_mockSettings.Setup(s => s.Value)
				.Returns(new OpenWeatherMapSettings
				{
					ApiKey = "test",
					BaseUri = ""
				});
			_mockLogger = new Mock<ILogger<GeocodingService>>();
		}

		[TestCase("emptyResponse", TestName = "Select first response from several responses from external API")]
		[TestCase("singleItemResponse", TestName = "Select the only returned response from external API")]
		[TestCase("severalItemResponse", TestName = "Return default result for an empty response from external API")]
		public async Task GetGeocodingCoordinatesByLocationAsync_ReturnsOneGeocodingResponse(string queryString)
		{
			// Arrange
			var serializedExpectedResponse = JsonConvert.SerializeObject(_requestResponseDictionary[queryString]);
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.Is<HttpRequestMessage>(m => m.RequestUri.Query.Contains(queryString))))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(serializedExpectedResponse)
				});

			var service = new GeocodingService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object);
			var request = new GeocodingRequest(queryString);

			//Act
			var result = await service.GetGeocodingCoordinatesByLocationAsync(request);

			//Assert
			var expectedResult = _requestResponseDictionary[queryString].FirstOrDefault();
			Assert.NotNull(result);
			Assert.AreEqual(expectedResult.Country, result.Country);
			Assert.AreEqual(expectedResult.State, result.State);
			Assert.AreEqual(expectedResult.Name, result.Name);
			Assert.AreEqual(expectedResult.Lon, result.Lon);
			Assert.AreEqual(expectedResult.Lat, result.Lat);
		}

		internal record ChangedGeocodingResponse(string Name, string Country, double Latitude, double Longitude){}

		[TestCase(TestName = "Throw an exception when extrenal service returns a different model")]
		public async Task GetGeocodingCoordinatesByLocationAsync_ThrowException_WhenExternalContractChanges()
		{
			// Arrange
			var serializedExpectedResponse = JsonConvert.SerializeObject(new ChangedGeocodingResponse("test", "te", 10.0d, 10.0d));
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new StringContent(serializedExpectedResponse)
				});

			var service = new GeocodingService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object);
			var request = new GeocodingRequest("test");

			//Act, Assert
			var ex = Assert.ThrowsAsync<JsonSerializationException>(async() => await service.GetGeocodingCoordinatesByLocationAsync(request));
		}

		[TestCase(TestName = "Throw an exception when extrenal service does not respond")]
		public void GetGeocodingCoordinatesByLocationAsync_ThrowException_WhenExternalServiceUnavailable()
		{
			// Arrange
			_mockHttpMessageHandler
				.Setup(h => h.Send(It.IsAny<HttpRequestMessage>()))
				.Throws(() => new TimeoutException("External service did not repond."));

			var service = new GeocodingService(_mockLogger.Object, _mockHttpCLientFactory.Object, _mockSettings.Object);
			var request = new GeocodingRequest("test");

			//Act, Assert
			var ex = Assert.ThrowsAsync<TimeoutException>(async () => await service.GetGeocodingCoordinatesByLocationAsync(request));
		}
	}
}
