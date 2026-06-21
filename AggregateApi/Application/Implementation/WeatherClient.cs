using Microsoft.Extensions.Options;  

public class WeatherClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;

    public WeatherClient(HttpClient httpClient, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<object> GetWeatherAsync(string country)
    {
        var url = $"current.json?key={_options.ApiKey}&q={country}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}