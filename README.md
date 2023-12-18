# CoreConcerns

CoreConcerns is a .NET library designed to provide a simplified approach to common cross-cutting concerns such as caching, logging, and validation.

## Features

- In-memory caching with easy setup and integration.
- Extendable caching strategies.
- Simple and intuitive API.

## Getting Started

### Installation

To install CoreConcerns, use the following NuGet command:

```shell
dotnet add package CoreConcerns
```

### Usage
Setting up Caching

In your `Startup.cs` or `Program.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInMemoryCacheProvider();
}
```

Using Caching in Your Application

Inject `ICacheProvider` into your services or controllers:
```csharp
public class MyService
{
    private readonly ICacheProvider _cacheProvider;

    public MyService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public async Task MyMethod()
    {
        // Use _cacheProvider methods here
    }
}
```

## Contributing
We welcome contributions! Please read our contributing guidelines before submitting pull requests.