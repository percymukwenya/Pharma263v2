using System;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    /// <summary>
    /// Cache service for storing and retrieving frequently accessed data
    /// Implements cache-aside pattern for reference data optimization
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets a cached value or creates it if not found
        /// </summary>
        /// <typeparam name="T">Type of cached data</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function to create data if cache miss</param>
        /// <param name="absoluteExpirationMinutes">Absolute expiration in minutes (default: 30)</param>
        /// <param name="slidingExpirationMinutes">Sliding expiration in minutes (default: 10)</param>
        /// <returns>Cached or newly created data</returns>
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            int absoluteExpirationMinutes = 30,
            int slidingExpirationMinutes = 10);

        /// <summary>
        /// Gets a value from cache
        /// </summary>
        /// <typeparam name="T">Type of cached data</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or default</returns>
        T Get<T>(string key);

        /// <summary>
        /// Sets a value in cache
        /// </summary>
        /// <typeparam name="T">Type of data to cache</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="absoluteExpirationMinutes">Absolute expiration in minutes</param>
        /// <param name="slidingExpirationMinutes">Sliding expiration in minutes</param>
        void Set<T>(string key, T value, int absoluteExpirationMinutes = 30, int slidingExpirationMinutes = 10);

        /// <summary>
        /// Removes a specific key from cache
        /// </summary>
        /// <param name="key">Cache key to remove</param>
        void Remove(string key);

        /// <summary>
        /// Removes all keys matching a pattern (e.g., "medicines:*")
        /// </summary>
        /// <param name="pattern">Pattern to match (supports * wildcard)</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clears all cache entries
        /// </summary>
        void Clear();
    }
}
