using AggregateApi.Application.Interfaces;
using AggregateApi.Model;

namespace AggregateApi.Application.Implementation
{
    public class AggregateService(IApiService apiService, ICacheService cacheService) : IAggregateService
    {
        public async Task<AggregateResponse> GetAggregateDataAsync(string? date, string? sortBy, string? company, string? country)
        {
            // Apply default values
            var effectiveDate = date;
            var effectiveSortBy = string.IsNullOrWhiteSpace(sortBy) ? "relevance" : sortBy;
            var effectiveCompany = string.IsNullOrWhiteSpace(company) ? "DefaultCompany" : company;
            var effectiveCountry = string.IsNullOrWhiteSpace(country) ? "us" : country;

            string weatherApiUrl = $"http://api.weatherapi.com/v1/current.json?key=efb1e101a69f4fc4b93132147251101&q={effectiveCountry}";
            string newsApiUrl = $"https://newsapi.org/v2/everything?q={effectiveCompany}&from={effectiveDate:yyyy-MM-dd}&sortBy={effectiveSortBy}&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";

            string weatherCacheKey = $"Weather_{effectiveCountry}";
            string newsCacheKey = $"News_{effectiveCompany}_{effectiveDate}_{effectiveSortBy}";

            // Add fallback data
            var fallbackWeatherData = new { Message = "Weather data not available. Using fallback." };
            var fallbackNewsData = new { Message = "News data not available. Using fallback." };

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

            var results = await Task.WhenAll(weatherTask, newsTask);

            return new AggregateResponse
            {
                WeatherApiData = results[0],
                NewsApiData = results[1]
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
