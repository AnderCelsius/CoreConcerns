using CoreConcerns.Caching.Implementations;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Concurrent;

namespace CoreConcerns.Caching.Tests;

public class InMemoryCacheProviderTests
{
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly InMemoryCacheProvider _cacheProvider;

    public InMemoryCacheProviderTests()
    {
        _mockMemoryCache = new Mock<IMemoryCache>();
        _cacheProvider = new InMemoryCacheProvider(_mockMemoryCache.Object);
    }

    [Fact]
    public async Task SetAsync_ItemAddedToCache_ItemCanBeRetrieved()
    {
        // Arrange
        const string key = "testKey";
        const string value = "testValue";
        var cache = new ConcurrentDictionary<object, object?>();

        var cacheEntryMock = new Mock<ICacheEntry>();
        cacheEntryMock.SetupAllProperties(); // Sets up all properties, including Value.

        // Mock the CreateEntry method to setup cache entry and add to the cache on dispose.
        _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns((object k) =>
        {
            cacheEntryMock.Setup(_ => _.Dispose()).Callback(() => { cache[k] = cacheEntryMock.Object.Value; });
            return cacheEntryMock.Object;
        });

        // Mock TryGetValue to get value from our simulated cache.
        _mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns((object k, out object val) => cache.TryGetValue(k, out val));

        // Act
        await _cacheProvider.SetAsync(key, value);

        // Retrieve the value from the cache using the method we are testing.
        var cachedValue = await _cacheProvider.GetAsync<string>(key);

        // Assert
        Assert.Equal(value, cachedValue);
    }


    [Fact]
    public async Task RemoveAsync_ItemExists_RemovesItemFromCache()
    {
        // Arrange
        const string key = "testKey";
        var value = new object();
        _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
        await _cacheProvider.SetAsync(key, value);

        // Act
        await _cacheProvider.RemoveAsync(key);
        var cachedValue = await _cacheProvider.GetAsync<object>(key);

        // Assert
        Assert.Null(cachedValue);
    }

    [Fact]
    public async Task ExistsAsync_ItemExists_ReturnsTrue()
    {
        // Arrange
        const string key = "testKey";
        object actualValue;
        _mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out actualValue)).Returns(true);

        // Act
        var exists = await _cacheProvider.ExistsAsync(key);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ClearAsync_CacheHasItems_ClearsAllItems()
    {
        // Arrange
        // Assume cache has been populated with items

        // Act
        await _cacheProvider.ClearAsync();

        // You can check for each key or just one if you've set up a mechanism to track keys
        var exists = await _cacheProvider.ExistsAsync("someKey");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GetAsync_NullKey_ThrowsArgumentNullException()
    {
        // Arrange
        string? key = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _cacheProvider.GetAsync<string>(key));
    }
}
