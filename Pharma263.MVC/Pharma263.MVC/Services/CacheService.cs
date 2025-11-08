using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.MVC.Services.IService;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Cache service implementation using IMemoryCache
    /// Provides cache-aside pattern for reference data with automatic expiration
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        // Track cache keys for pattern-based removal
        private static readonly ConcurrentDictionary<string, byte> _cacheKeys = new();

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            int absoluteExpirationMinutes = 30,
            int slidingExpirationMinutes = 10)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            // Try to get from cache first
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                _logger.LogDebug("Cache HIT for key: {CacheKey}", key);
                return cachedValue;
            }

            // Cache miss - create value
            _logger.LogDebug("Cache MISS for key: {CacheKey}", key);

            var value = await factory();

            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes),
                Priority = CacheItemPriority.Normal
            };

            // Add eviction callback for logging
            cacheOptions.RegisterPostEvictionCallback((cacheKey, cacheValue, reason, state) =>
            {
                _logger.LogDebug("Cache entry evicted. Key: {CacheKey}, Reason: {Reason}", cacheKey, reason);
                _cacheKeys.TryRemove(cacheKey.ToString(), out _);
            });

            // Set in cache and track key
            _cache.Set(key, value, cacheOptions);
            _cacheKeys.TryAdd(key, 0);

            _logger.LogInformation("Cached data for key: {CacheKey}, Expires in {Minutes} minutes",
                key, absoluteExpirationMinutes);

            return value;
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_cache.TryGetValue(key, out T value))
            {
                _logger.LogDebug("Cache GET HIT for key: {CacheKey}", key);
                return value;
            }

            _logger.LogDebug("Cache GET MISS for key: {CacheKey}", key);
            return default;
        }

        public void Set<T>(string key, T value, int absoluteExpirationMinutes = 30, int slidingExpirationMinutes = 10)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes),
                Priority = CacheItemPriority.Normal
            };

            cacheOptions.RegisterPostEvictionCallback((cacheKey, cacheValue, reason, state) =>
            {
                _logger.LogDebug("Cache entry evicted. Key: {CacheKey}, Reason: {Reason}", cacheKey, reason);
                _cacheKeys.TryRemove(cacheKey.ToString(), out _);
            });

            _cache.Set(key, value, cacheOptions);
            _cacheKeys.TryAdd(key, 0);

            _logger.LogDebug("Cache SET for key: {CacheKey}", key);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
            _cacheKeys.TryRemove(key, out _);

            _logger.LogInformation("Cache entry removed: {CacheKey}", key);
        }

        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(nameof(pattern));

            // Convert pattern to regex (simple * wildcard support)
            var regex = new System.Text.RegularExpressions.Regex(
                "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*") + "$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var keysToRemove = _cacheKeys.Keys.Where(k => regex.IsMatch(k)).ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }

            _logger.LogInformation("Removed {Count} cache entries matching pattern: {Pattern}",
                keysToRemove.Count, pattern);
        }

        public void Clear()
        {
            var keys = _cacheKeys.Keys.ToList();

            foreach (var key in keys)
            {
                _cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }

            _logger.LogWarning("Cache cleared. Removed {Count} entries.", keys.Count);
        }
    }
}
