using AggregateApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http;
using System.Text.Json;

namespace AggregateApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregateController(HttpClient client) : ControllerBase
    {

        [HttpGet]
        [SwaggerOperation(Summary = "Fetches weather and news data from external APIs",
         Description = "This endpoint calls two external APIs (weather and news), handles them asynchronously, and returns an aggregate response with weather and news data.")]
        public async Task<ActionResult<AggregateResponse>> Get(
            [FromQuery, SwaggerParameter(Description = "The date for the news articles (used for filtering).")] string? date,
            [FromQuery, SwaggerParameter(Description = "Field to sort news articles by.")] string? sortBy = null,
            [FromQuery, SwaggerParameter(Description = "Sort order for the articles: 'asc' or 'desc'. Default is 'asc'.")] string? company = "Apple",
            [FromQuery, SwaggerParameter(Description = "Filter weather by country.")] string? country = null
)
        {

            if (date == null)
            {
                return BadRequest("The 'date' query parameter is required.");
            }

            if (string.IsNullOrEmpty(company))
            {
                return BadRequest("The Company query parameter is required.");
            }
            if (sortBy == null) 
            {
                return BadRequest("The sort query parameter is required.");
            }
            if (country == null)
            {
                return BadRequest("The country query parameter is required.");
            }


            var newdate = DateTime.Parse(date);
            var formattedDate = newdate.ToString("yyyy-MM-ddTHH:mm:ssZ"); // Adjust format as required
            var encodedDate = Uri.EscapeDataString(formattedDate);

            var externalApiUrl = $"https://newsapi.org/v2/everything?q={company}&from={encodedDate}&sortBy={sortBy}&apiKey=7b66f419b1a04be1b8cd5364a4d2dfa4";
            var externalApiUrl2 = $"http://api.weatherapi.com/v1/current.json?key=efb1e101a69f4fc4b93132147251101&q={country}";

            try
            {
                
                    var tasks = new[]
                    {
                      FetchApiDataWithFallbackAsync(externalApiUrl, new { Message = "News data fallback." }),
                      FetchApiDataWithFallbackAsync(externalApiUrl2, new { Message = "Weather data fallback." }),
                     
                    };

                    var results = await Task.WhenAll(tasks);
                    if (results[0] == null)
                    {
                        return StatusCode(500, "Failed to fetch data from one or more external APIs.");
                    }

                    var response = new AggregateResponse
                    {
                        WeatherApiData = results[1],
                        NewsApiData = results[0],
                    };

                    return Ok(response);
               
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error fetching data: {ex.Message}");
            }
        }


        private async Task<object> FetchApiDataWithFallbackAsync(string apiUrl, object fallbackData)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(600)); 
                var response = await client.GetAsync(apiUrl, cts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error fetching data from {apiUrl}: HTTP Status Code {response.StatusCode}");
                    return fallbackData;
                }

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<object>(json);

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Request to {apiUrl} timed out.");
                return fallbackData;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Error fetching data from {apiUrl}: {httpEx.Message}");
                return fallbackData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error fetching data from {apiUrl}: {ex.Message}");
                return fallbackData;
            }
        }

    }
}
