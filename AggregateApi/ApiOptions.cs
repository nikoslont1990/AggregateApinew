namespace AggregateApi
{
    public class ApiOptions
    {
        public NewsOptions News { get; set; }
        public WeatherOptions Weather { get; set; }
        public TwitterOptions Twitter { get; set; }
    }
    public class NewsOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class WeatherOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class TwitterOptions
    {
        public string BaseUrl { get; set; }
    }
}