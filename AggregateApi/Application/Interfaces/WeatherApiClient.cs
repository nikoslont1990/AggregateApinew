using System.Text.Json;

namespace AggregateApi.Application.Interfaces
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;

        public WeatherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherApiClient> GetCurrentWeatherAsync(string location)
        {
            try
            {
                var url = $"http://api.weatherapi.com/v1/current.json?key=efb1e101a69f4fc4b93132147251101&q={location}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // Return this instance for fluent API or chaining
                return this;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching weather for {location}: {ex.Message}");
                return this;
            }
        }
    }
}
