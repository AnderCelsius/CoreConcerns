namespace CoreConcerns.Caching;

public interface ICacheProvider
{
    /// <summary>
    /// Asynchronously retrieves an item of type <typeparamref name="T"/> from the cache using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the item in the cache.</typeparam>
    /// <param name="key">The unique key corresponding to the item.</param>
    /// <returns>The cached item if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
    Task<T?> GetAsync<T>(string key) where T : class;

    /// <summary>
    /// Asynchronously adds an item of type <typeparamref name="T"/> to the cache with the specified key, or updates the value if the key already exists.
    /// </summary>
    /// <typeparam name="T">The type of the item to cache.</typeparam>
    /// <param name="key">The unique key to store the item under.</param>
    /// <param name="value">The item to cache.</param>
    /// <param name="absoluteExpireTime">The absolute expiration time for the item or null for no expiration.</param>
    /// <param name="unusedExpireTime">The sliding expiration timespan for the item or null for no expiration.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetAsync<T>(string? key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);

    /// <summary>
    /// Asynchronously removes the cached item associated with the specified key.
    /// </summary>
    /// <param name="key">The unique key corresponding to the item to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(string? key);

    /// <summary>
    /// Asynchronously checks if an item associated with the specified key exists in the cache.
    /// </summary>
    /// <param name="key">The key to check in the cache.</param>
    /// <returns>A task that completes with true if the item exists, or false otherwise.</returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Asynchronously clears all items from the cache.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation of clearing the cache.</returns>
    Task ClearAsync();
}