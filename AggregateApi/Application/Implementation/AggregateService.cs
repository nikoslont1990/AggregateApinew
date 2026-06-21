using AggregateApi.Application.Interfaces;
using AggregateApi.Model;
using static System.Net.WebRequestMethods;

namespace AggregateApi.Application.Implementation
{
    public class AggregateService(IApiService apiService, ICacheService cacheService) : IAggregateService
    {
        
        public async Task<AggregateResponse> GetAggregateDataAsync(string? date, string? sortBy, string? company, string? country,string? category,string? url)
        {
            // Apply default values
            var effectiveDate = date;
            var effectiveSortBy = string.IsNullOrWhiteSpace(sortBy) ? "relevance" : sortBy;
            var effectiveCompany = string.IsNullOrWhiteSpace(company) ? "DefaultCompany" : company;
            var effectiveCountry = string.IsNullOrWhiteSpace(country) ? "us" : country;

            var weatherApiUrl = $"http://api.weatherapi.com/v1/current.json?key=efb1e101a69f4fc4b93132147251101&q={effectiveCountry}";
            var newsApiUrl = $"https://newsapi.org/v2/everything?q={effectiveCompany}&from={effectiveDate:yyyy-MM-dd}&to={effectiveDate:yyyy-MM-dd}&sortBy={effectiveSortBy}&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";
            var newsApiUrl1 = $"https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";
            //string? url1 = "https://twitter.com/Interior/status/507185938620219395";
            var twitterApiUrl = $"https://publish.twitter.com/oembed?url={url}";
            var weatherCacheKey = $"Weather_{effectiveCountry}";
            var newsCacheKey = $"News_{effectiveCompany}_{effectiveDate}_{effectiveSortBy}";
            var newsCacheKey1 = $"News_{effectiveCountry}_{category}";
            var twitterCacheKey = $"Twitter_{url}";

            //lalallala
            // Add fallback data
            var fallbackWeatherData = new { Message = "Weather data not available. Using fallback." };
            var fallbackNewsData = new { Message = "News data not available. Using fallback." };
            var fallbacktwitterData = new { Message = "Twitter data not available. Using fallback." };

            // Fetch data with fallback mechanism
            var weatherTask = cacheService.GetOrCreateAsync(
                weatherCacheKey,
                () => FetchWithFallbackAsync(weatherApiUrl, "Web_Api_Client", fallbackWeatherData),
                TimeSpan.FromMinutes(10)
            );

            var newsTask = cacheService.GetOrCreateAsync(
                newsCacheKey,
                () => FetchWithFallbackAsync(newsApiUrl, "Web_Api_Client", fallbackNewsData),
                TimeSpan.FromMinutes(10)
            );

            var newsTaskcateg = cacheService.GetOrCreateAsync(
               newsCacheKey,
               () => FetchWithFallbackAsync(newsApiUrl1, "Web_Api_Client", fallbackNewsData),
               TimeSpan.FromMinutes(10)
           );
            var twitterTask = cacheService.GetOrCreateAsync(
             twitterCacheKey,
             () => FetchWithFallbackAsync(twitterApiUrl, "Web_Api_Client", fallbackNewsData),
             TimeSpan.FromMinutes(10)
         );
            var results = await Task.WhenAll(weatherTask, newsTask, newsTaskcateg, twitterTask);
            var result = weatherTask;
            return new AggregateResponse
            {
                WeatherApiData = results[0],
                NewsApiData = results[1],
                NewsApiCategoryData = results[2],
                TwitterData = results[3],
            };
        }

        private async Task<object> FetchWithFallbackAsync(string name,string url, object fallbackData)
        {
            try
            {
                return await apiService.FetchApiDataAsync<object>(name,url);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching data from {name}: {ex.Message}");
                return fallbackData;
            }
        }
    }
}
