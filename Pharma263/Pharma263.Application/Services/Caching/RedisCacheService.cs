using Pharma263.Application.Contracts.Caching;

namespace Pharma263.Application.Services.Caching
{
    public class RedisCacheService : ICacheService
    {
        public void Remove(string cacheKey)
        {
            throw new System.NotImplementedException();
        }

        public T Set<T>(string cacheKey, T value)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            throw new System.NotImplementedException();
        }
    }
}
