using CoreConcerns.Caching.Implementations;
using CoreConcerns.Caching.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CoreConcerns.Caching.Extensions;

public static class CacheServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryCacheProvider(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();
        return services;
    }

    public static IServiceCollection AddRedisCacheProvider(this IServiceCollection services, string connectionString)
    {
        var redis = ConnectionMultiplexer.Connect(connectionString);
        services.AddSingleton<IConnectionMultiplexer>(redis);
        services.AddSingleton<ICacheProvider, RedisCacheProvider>();
        return services;
    }
}
