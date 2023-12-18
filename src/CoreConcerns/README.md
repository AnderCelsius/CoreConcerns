# CoreConcerns.Caching

This library provides caching solutions as part of the CoreConcerns suite, supporting both in-memory and distributed Redis caching.

## Features

- Easy setup and integration with .NET applications.
- Support for in-memory caching suitable for single-instance applications.
- Support for Redis caching suitable for distributed applications.

## Getting Started

### Installation

```shell
dotnet add package CoreConcerns.Caching
```

### Configuration

#### In-Memory Caching

Register in-memory caching in `Startup.cs` or `Program.cs`:

```csharp
services.AddInMemoryCacheProvider();
```

#### Redis Caching

For Redis caching, provide a connection string during registration:

```csharp
services.AddRedisCacheProvider("your_redis_connection_string");
```

### Usage

Inject `ICacheProvider` into your services to utilize the caching features:

```csharp
public class MyService
{
    private readonly ICacheProvider _cacheProvider;

    public MyService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    // Example method demonstrating caching usage.
    public async Task MyMethod()
    {
        // Your caching logic here
    }
}
```

