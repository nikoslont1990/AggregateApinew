public class NewsClient
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiOptions _options;

    public NewsClient(HttpClient httpClient, IOptions<NewsApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<object> GetNewsAsync(string company, DateTime? date, string sortBy)
    {
        var url = $"everything?q={company}&from={date:yyyy-MM-dd}&to={date:yyyy-MM-dd}&sortBy={sortBy}&apiKey={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<object> GetTopHeadlinesAsync(string country, string category)
    {
        var url = $"top-headlines?country={country}&category={category}&apiKey={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}