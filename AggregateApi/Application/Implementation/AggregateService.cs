using AggregateApi.Application.Interfaces;
using AggregateApi.Model;

namespace AggregateApi.Application.Implementation
{
    public class AggregateService(IApiService apiService, ICacheService cacheService) : IAggregateService
    {
        public async Task<AggregateResponse> GetAggregateDataAsync(string? date, string? sortBy, string? company, string? country,string? category,string url)
        {
            // Apply default values
            var effectiveDate = date;
            var effectiveSortBy = string.IsNullOrWhiteSpace(sortBy) ? "relevance" : sortBy;
            var effectiveCompany = string.IsNullOrWhiteSpace(company) ? "DefaultCompany" : company;
            var effectiveCountry = string.IsNullOrWhiteSpace(country) ? "us" : country;

            string weatherApiUrl = $"http://api.weatherapi.com/v1/current.json?key=efb1e101a69f4fc4b93132147251101&q={effectiveCountry}";
            string newsApiUrl = $"https://newsapi.org/v2/everything?q={effectiveCompany}&from={effectiveDate:yyyy-MM-dd}&to={effectiveDate:yyyy-MM-dd}&sortBy={effectiveSortBy}&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";
            string newsApiUrl1 = $"https://newsapi.org/v2/top-headlines?country=us&category=business&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";
            string twitterApiUrl = $"https://publish.twitter.com/oembed?url={url}";
            string weatherCacheKey = $"Weather_{effectiveCountry}";
            string newsCacheKey = $"News_{effectiveCompany}_{effectiveDate}_{effectiveSortBy}";
            string newsCacheKey1 = $"News_{effectiveCountry}_{category}";
            string twitterCacheKey = $"Twitter_{url}";

            // Add fallback data
            var fallbackWeatherData = new { Message = "Weather data not available. Using fallback." };
            var fallbackNewsData = new { Message = "News data not available. Using fallback." };
            var fallbacktwitterData = new { Message = "Twitter data not available. Using fallback." };

            // Fetch data with fallback mechanism
            var weatherTask = cacheService.GetOrCreateAsync(
                weatherCacheKey,
                () => FetchWithFallbackAsync(weatherApiUrl, fallbackWeatherData),
                TimeSpan.FromMinutes(10)
            );

            var newsTask = cacheService.GetOrCreateAsync(
                newsCacheKey,
                () => FetchWithFallbackAsync(newsApiUrl, fallbackNewsData),
                TimeSpan.FromMinutes(10)
            );

            var newsTaskcateg = cacheService.GetOrCreateAsync(
               newsCacheKey,
               () => FetchWithFallbackAsync(newsApiUrl1, fallbackNewsData),
               TimeSpan.FromMinutes(10)
           );
            var twitterTask = cacheService.GetOrCreateAsync(
             twitterCacheKey,
             () => FetchWithFallbackAsync(twitterApiUrl, fallbackNewsData),
             TimeSpan.FromMinutes(10)
         );
            var results = await Task.WhenAll(weatherTask, newsTask, newsTaskcateg, twitterTask);

            return new AggregateResponse
            {
                WeatherApiData = results[0],
                NewsApiData = results[1],
                NewsApiCategoryData = results[2],
                TwitterData = results[3],
            };
        }

        private async Task<object> FetchWithFallbackAsync(string apiUrl, object fallbackData)
        {
            try
            {
                return await apiService.FetchApiDataAsync<object>(apiUrl);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching data from {apiUrl}: {ex.Message}");
                return fallbackData;
            }
        }
    }
}
