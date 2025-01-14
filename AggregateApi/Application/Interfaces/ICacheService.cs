namespace AggregateApi.Application.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchData, TimeSpan expiration);
    }
}
