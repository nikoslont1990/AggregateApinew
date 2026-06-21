//using AggregateApi.Application.Implementation;
//using AggregateApi.Application.Interfaces;
//using AggregateApi.Handler;
//using Moq;
//using System.ComponentModel.DataAnnotations;
//using System.Net;
//using System.Text;
//using System.Text.Json;

//namespace AggregateServiceTests
//{
//    public class AggregateApiTets
//    {
//        private readonly Mock<IApiService> _apiServiceMock;
//        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
//        private readonly Mock<ICacheService> _cacheServiceMock;
//        private readonly AggregateService _aggregateService;

//        public AggregateApiTets()
//        {
//            _apiServiceMock = new Mock<IApiService>();
//            _cacheServiceMock = new Mock<ICacheService>();
//            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
//            _aggregateService = new AggregateService(_apiServiceMock.Object, _cacheServiceMock.Object);
//        }

//        [Fact]
//        public async Task GetAggregateDataAsync_ValidRequest_ReturnsAggregateResponse()
//        {
//            // Arrange
//            var validDate = "2025-1-1";
//            var sortBy = "popularity";
//            var company = "Apple";
//            var country = "us";
//            var category = "popularity";
//            string? url = "https://twitter.com/Interior/status/507185938620219395";

//            // Mock HttpClient for Weather API
//            var weatherApiResponse = new { temp = "20°C" };
//            var weatherApiMessage = new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(JsonSerializer.Serialize(weatherApiResponse), Encoding.UTF8, "application/json")
//            };

//            var weatherHttpClient = new HttpClient(new MockHttpMessageHandler(weatherApiMessage));
//            _httpClientFactoryMock
//                .Setup(f => f.CreateClient("WeatherApi"))
//                .Returns(weatherHttpClient);

//            // Mock HttpClient for News API
//            var newsApiResponse = new { articles = new[] { new { title = "Apple News" } } };
//            var newsApiMessage = new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(JsonSerializer.Serialize(newsApiResponse), Encoding.UTF8, "application/json")
//            };

//            var newsHttpClient = new HttpClient(new MockHttpMessageHandler(newsApiMessage));
//            _httpClientFactoryMock
//                .Setup(f => f.CreateClient("NewsApi"))
//                .Returns(newsHttpClient);

//            // Act
//            var result = await _aggregateService.GetAggregateDataAsync(validDate, sortBy, company, country,category, url);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("20°C", result.WeatherApiData.ToString());
//            Assert.Contains("Apple News", result.NewsApiData.ToString());
//        }

//        //[Fact]
//        //public async Task GetAggregateDataAsync_ApiFails_UsesFallback()
//        //{
//        //    // Arrange
//        //    var validDate = "2025-1-1";
//        //    var sortBy = "popularity";
//        //    var company = "Apple";
//        //    var country = "us";
//        //    var category = "popularity";
//        //    string? url = "https://twitter.com/Interior/status/507185938620219395";

//        //    // Simulate API failure
//        //    var failedApiMessage = new HttpResponseMessage
//        //    {
//        //        StatusCode = HttpStatusCode.InternalServerError
//        //    };

//        //    var failingHttpClient = new HttpClient(new MockHttpMessageHandler(failedApiMessage));
//        //    _httpClientFactoryMock
//        //        .Setup(f => f.CreateClient(It.IsAny<string>()))
//        //        .Returns(failingHttpClient);

//        //    // Mock fallback service
//        //    var fallbackWeatherData = new { temp = "Fallback 20°C" };
//        //    var fallbackNewsData = new { articles = new[] { new { title = "Fallback Apple News" } } };

//        //    _fallbackServiceMock
//        //        .Setup(f => f.GetWeatherFallbackAsync())
//        //        .ReturnsAsync(fallbackWeatherData);

//        //    _fallbackServiceMock
//        //        .Setup(f => f.GetNewsFallbackAsync())
//        //        .ReturnsAsync(fallbackNewsData);

//        //    // Act
//        //    var result = await _aggregateService.GetAggregateDataAsync(validDate, sortBy, company, country,category,url);

//        //    // Assert
//        //    Assert.NotNull(result);
//        //    Assert.Contains("Fallback 20°C", result.WeatherApiData.ToString());
//        //    Assert.Contains("Fallback Apple News", result.NewsApiData.ToString());
//        //}

//        //[Fact]
//        //public async Task GetAggregateDataAsync_NullDate_ThrowsValidationException()
//        //{
//        //    // Arrange
//        //    DateTime? nullDate = null;
//        //    var sortBy = "popularity";
//        //    var company = "Apple";
//        //    var country = "us";
//        //    var category = "popularity";
//        //    string? url = "https://twitter.com/Interior/status/507185938620219395";

//        //    // Act & Assert
//        //    await Assert.ThrowsAsync<ValidationException>(() =>
//        //        _aggregateService.GetAggregateDataAsync(nullDate.Value, sortBy, company, country,category,url));
//        //}
//    }
//}