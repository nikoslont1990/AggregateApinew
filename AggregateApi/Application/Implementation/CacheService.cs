using AggregateApi.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AggregateApi.Application.Implementation
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchData, TimeSpan expiration)
        {
            if (memoryCache.TryGetValue(key, out T cachedData))
            {
                return cachedData;
            }

            var data = await fetchData();
            memoryCache.Set(key, data, expiration);

            return data;
        }
    }
}
