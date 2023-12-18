using CoreConcerns.Caching;

namespace CoreConcerns.Example.Services;

public class DataService
{
    private readonly ICacheProvider _cacheProvider;

    public DataService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public async Task<string?> GetCachedDataAsync(string key)
    {
        return await _cacheProvider.GetAsync<string>(key);
    }

    public async Task SetDataAsync(string key, string data)
    {
        // Set the provided data in the cache
        await _cacheProvider.SetAsync(key, data, TimeSpan.FromMinutes(10));
    }
}

