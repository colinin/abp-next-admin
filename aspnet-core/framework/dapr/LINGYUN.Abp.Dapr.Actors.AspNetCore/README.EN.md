# LINGYUN.Abp.Dapr.Actors.AspNetCore

Integration of Dapr.Actors with ASP.NET Core in the ABP framework. This module automatically scans and registers Actor services defined within assemblies as Dapr.Actors.

## Features

* Automatic Actor service registration
* Integration with ABP's dependency injection system
* Support for custom Actor type names through `RemoteServiceAttribute`
* Actor runtime configuration through `ActorRuntimeOptions`
* Automatic Actor endpoint mapping
* Actor interface validation

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(typeof(AbpDaprActorsAspNetCoreModule))]
public class YourProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // Configure Actor runtime options
        PreConfigure<ActorRuntimeOptions>(options =>
        {
            options.ActorIdleTimeout = TimeSpan.FromMinutes(60);
            options.ActorScanInterval = TimeSpan.FromSeconds(30);
            options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(30);
            options.DrainRebalancedActors = true;
            options.RemindersStoragePartitions = 7;
        });
    }
}
```

## Implementation Example

1. Define Actor Interface

```csharp
[RemoteService("counter")] // Optional: customize Actor type name
public interface ICounterActor : IActor
{
    Task<int> GetCountAsync();
    Task IncrementCountAsync();
}
```

2. Implement Actor

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

The module will automatically:
1. Detect the `CounterActor` implementation
2. Register it with Dapr.Actors
3. Configure the Actor runtime
4. Map the Actor endpoints

## Actor Runtime Configuration

The module supports all standard Dapr Actor runtime configurations through `ActorRuntimeOptions`:

```csharp
PreConfigure<ActorRuntimeOptions>(options =>
{
    // Actor timeout settings
    options.ActorIdleTimeout = TimeSpan.FromMinutes(60);
    options.ActorScanInterval = TimeSpan.FromSeconds(30);
    
    // Draining settings
    options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(30);
    options.DrainRebalancedActors = true;
    
    // Reminders settings
    options.RemindersStoragePartitions = 7;
    
    // Custom serialization settings
    options.JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
});
```

## Important Notes

* Actor implementations must be registered in the dependency injection container
* Actor interfaces must inherit from `IActor`
* Actor type names can be customized using the `RemoteServiceAttribute`
* The module automatically maps Actor endpoints using ABP's endpoint routing system
* Actor runtime options should be configured in the `PreConfigureServices` phase

## Endpoint Mapping

The module automatically maps the following Actor endpoints:
* `/dapr/actors/{actorType}/{actorId}/method/{methodName}`
* `/dapr/actors/{actorType}/{actorId}/state`
* `/dapr/actors/{actorType}/{actorId}/reminders/{reminderName}`
* `/dapr/actors/{actorType}/{actorId}/timers/{timerName}`

[查看中文](README.md)
