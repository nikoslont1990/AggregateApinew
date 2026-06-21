namespace AggregateApi.Application.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<WeatherApiClient> GetCurrentWeatherAsync(string location);
    }
}
