using CoreConcerns.Caching.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoreConcerns.Caching.Implementations;

public class RedisCacheProvider : ICacheProvider
{
    private readonly IConnectionMultiplexer _redisConnection;

    public RedisCacheProvider(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var db = _redisConnection.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value);
    }


    public async Task SetAsync<T>(string? key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var db = _redisConnection.GetDatabase();
        var serializedValue = JsonConvert.SerializeObject(value);
        await db.StringSetAsync(key, serializedValue, absoluteExpireTime);
    }

    public Task RemoveAsync(string? key)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}
