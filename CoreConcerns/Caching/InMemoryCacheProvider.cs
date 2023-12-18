using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CoreConcerns.Caching;

public class InMemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;
    private readonly ConcurrentDictionary<string, byte> _keys = new();

    public InMemoryCacheProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache),
            "An instance of IMemoryCache must be provided.");
    }

    public Task<T> GetAsync<T>(string? key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key), "Cache key cannot be null.");

        // Attempt to get the item from the cache. If the key doesn't exist, default(T) is returned.
        _memoryCache.TryGetValue(key, out T cacheEntry);
        return Task.FromResult(cacheEntry);
    }

    public Task SetAsync<T>(string? key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        if (key == null) throw new ArgumentNullException(nameof(key), "Cache key cannot be null.");
        // Allow storing null values to support caching of null results if needed.

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = unusedExpireTime,
            AbsoluteExpirationRelativeToNow = absoluteExpireTime
        };

        // Save data in cache.
        _memoryCache.Set(key, value, cacheEntryOptions);
        _keys.TryAdd(key, 0);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string? key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key), "Cache key cannot be null.");

        _memoryCache.Remove(key);
        _keys.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key), "Cache key cannot be null.");

        return Task.FromResult(_memoryCache.TryGetValue(key, out _));
    }

    public Task ClearAsync()
    {
        var allKeys = _keys.Keys.ToArray();
        foreach (var key in allKeys)
        {
            _memoryCache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }
}