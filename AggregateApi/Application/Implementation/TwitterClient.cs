public class TwitterClient
{
    private readonly HttpClient _httpClient;

    public TwitterClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<object> GetTweetAsync(string url)
    {
        var response = await _httpClient.GetAsync($"oembed?url={url}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}