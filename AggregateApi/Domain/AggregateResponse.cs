namespace AggregateApi.Model
{
    public class AggregateResponse
    {
        public object WeatherApiData { get; set; }
        public object NewsApiData { get; set; }
        public object NewsApiCategoryData { get; set; }
        public object TwitterData { get; set; }
    }
}
