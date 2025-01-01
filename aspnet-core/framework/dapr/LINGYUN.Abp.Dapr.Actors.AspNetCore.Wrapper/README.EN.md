# LINGYUN.Abp.Dapr.Actors.AspNetCore.Wrapper

Dapr Actors ASP.NET Core wrapper module for handling Actor method call response wrapping and unwrapping.

## Features

* Automatic Actor response result wrapping/unwrapping
* Integration with ABP's wrapper system
* Error handling for Actor method calls
* Support for success/error code configuration
* Integration with Dapr.Actors.AspNetCore
* Support for custom response wrapper format
* Flexible wrapping control options

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(
    typeof(AbpDaprActorsAspNetCoreModule),
    typeof(AbpWrapperModule)
)]
public class YouProjectModule : AbpModule
{
}
```

## Configuration Options

The module uses `AbpWrapperOptions` from the `LINGYUN.Abp.Wrapper` package for configuration:

```json
{
    "Wrapper": {
        "IsEnabled": true,  // Enable/disable response wrapping
        "CodeWithSuccess": "0",  // Success code in wrapped response
        "HttpStatusCode": 200,  // Default HTTP status code for wrapped responses
        "WrapOnError": true,  // Whether to wrap error responses
        "WrapOnSuccess": true  // Whether to wrap success responses
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

## Response Format

When wrapping is enabled, Actor method responses will be in the following format:

```json
{
    "code": "0",  // Response code, "0" indicates success by default
    "message": "Success",  // Response message
    "details": null,  // Additional details (optional)
    "result": {  // Actual response data
        // ... Actor method return value
    }
}
```

## Error Handling

The module automatically handles Actor method call errors:
* For wrapped responses:
  * Unwraps the response and checks the code
  * If code doesn't match `CodeWithSuccess`, throws `AbpRemoteCallException`
  * Includes error message, details, and code in the exception
  * Supports custom error code mapping
* For Actor runtime errors:
  * Automatically wraps as standard error response
  * Preserves original exception information
  * Includes Actor-related context information

### Error Response Example

```json
{
    "code": "ERROR_001",
    "message": "Actor method call failed",
    "details": "Failed to access state for actor 'counter'",
    "result": null
}
```

## Advanced Usage

### 1. Controlling Response Wrapping

Response wrapping can be controlled per Actor call using HTTP headers:

```csharp
// Add to request headers
var headers = new Dictionary<string, string>
{
    { "X-Abp-Wrap-Result", "true" },  // Force enable wrapping
    // or
    { "X-Abp-Dont-Wrap-Result", "true" }  // Force disable wrapping
};

// Use in Actor method
public async Task<int> GetCountAsync()
{
    var context = ActorContext.GetContext();
    context.Headers.Add("X-Abp-Wrap-Result", "true");
    
    var count = await StateManager.TryGetStateAsync<int>(CountStateName);
    return count.HasValue ? count.Value : 0;
}
```

### 2. Custom Error Handling

```csharp
public class CustomActorErrorHandler : IAbpWrapperErrorHandler
{
    public Task HandleAsync(AbpWrapperErrorContext context)
    {
        if (context.Exception is ActorMethodInvocationException actorException)
        {
            // Custom Actor error handling logic
            context.Response = new WrapperResponse
            {
                Code = "ACTOR_ERROR",
                Message = actorException.Message,
                Details = actorException.ActorId
            };
        }
        return Task.CompletedTask;
    }
}
```

## Important Notes

* Response wrapping can be controlled through:
  * Global settings in configuration
  * HTTP headers for individual requests
  * Dynamic control in Actor methods
* Error responses maintain original error structure for Actor methods
* The module integrates with ABP's remote service error handling system
* Recommended to use response wrapping consistently in microservices architecture
* Wrapper format can be customized by implementing `IAbpWrapperResponseBuilder`
* Actor state operation errors are properly wrapped and handled

[查看中文](README.md)
