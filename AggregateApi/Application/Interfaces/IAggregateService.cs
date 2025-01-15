using AggregateApi.Model;

namespace AggregateApi.Application.Interfaces
{
    public interface IAggregateService
    {
        Task<AggregateResponse> GetAggregateDataAsync(string date, string? sortBy, string? company, string? country,string? category,string url);
    }
}
