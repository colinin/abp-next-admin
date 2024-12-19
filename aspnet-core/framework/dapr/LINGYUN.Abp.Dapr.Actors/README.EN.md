# LINGYUN.Abp.Dapr.Actors

Dapr.IActor client proxy module

## Features

* Dynamic proxy generation for Dapr Actors
* Integration with ABP's remote service system
* Support for Actor authentication and authorization
* Multi-tenant support
* Automatic request/response handling
* Custom error handling
* Culture and language header support

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(typeof(AbpDaprActorsModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register proxies similar to Volo.Abp.Http.Client module
        context.Services.AddDaprActorProxies(
            typeof(YouProjectActorInterfaceModule).Assembly, // Search for IActor definitions in YouProjectActorInterfaceModule
            RemoteServiceName
        );
    }
}
```

## Configuration Options

```json
{
    "RemoteServices": {
        "Default": {
            "BaseUrl": "http://localhost:3500",  // Required, Dapr HTTP endpoint
            "DaprApiToken": "your-api-token",    // Optional, Dapr API Token
            "RequestTimeout": "30000"            // Optional, request timeout in milliseconds (default: 30000)
        }
    }
}
```

## Implementation Example

1. Actor Interface Definition

```csharp
public interface ICounterActor : IActor
{
    Task<int> GetCountAsync();
    Task IncrementCountAsync();
}
```

2. Actor Implementation

```csharp
public class CounterActor : Actor, ICounterActor
{
    private const string CountStateName = "count";

    public CounterActor(ActorHost host) : base(host)
    {
    }

    public async Task<int> GetCountAsync()
    {
        var count = await StateManager.TryGetStateAsync<int>(CountStateName);
        return count.HasValue ? count.Value : 0;
    }

    public async Task IncrementCountAsync()
    {
        var currentCount = await GetCountAsync();
        await StateManager.SetStateAsync(CountStateName, currentCount + 1);
    }
}
```

3. Client Usage

```csharp
public class CounterService
{
    private readonly ICounterActor _counterActor;

    public CounterService(ICounterActor counterActor)
    {
        _counterActor = counterActor;
    }

    public async Task<int> GetAndIncrementCountAsync()
    {
        var count = await _counterActor.GetCountAsync();
        await _counterActor.IncrementCountAsync();
        return count;
    }
}
```

## Important Notes

* Actor methods must return `Task` or `Task<T>`
* Actor methods can have at most one parameter
* Actor instances are single-threaded, processing one request at a time
* Actor state is managed by the Dapr runtime
* The module automatically handles:
  * Authentication headers
  * Tenant context
  * Culture information
  * Request timeouts
  * Error handling

## Error Handling

The module provides custom error handling for Actor calls:
* `AbpDaprActorCallException`: Thrown when an Actor method call fails
* `ActorMethodInvocationException`: Contains detailed information about the failure
* Error responses include:
  * Error message
  * Error code
  * Original exception type

[查看中文](README.md)
