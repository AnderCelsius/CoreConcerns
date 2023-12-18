using CoreConcerns.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace CoreConcerns.Extensions;

public static class CacheServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryCacheProvider(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();
        return services;
    }
}
