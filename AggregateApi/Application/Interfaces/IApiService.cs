using AggregateApi.Model;

namespace AggregateApi.Application.Interfaces
{
    public interface IApiService
    {
        Task<T> FetchApiDataAsync<T>(string url);
    }
}
