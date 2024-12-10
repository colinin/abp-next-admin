# LINGYUN.Abp.Dapr

Dapr integration base module, implementing the named singleton DaprClient as described in the Dapr documentation.

See: https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-client/dotnet-daprclient-usage

## Features

* Support for creating default and named DaprClient instances
* Support for configuring HTTP and gRPC endpoints
* Support for custom JSON serialization options
* Support for Dapr API Token authentication
* Support for gRPC channel configuration
* Support for DaprClient instance configuration and builder configuration extensions
* Support for multiple Dapr Sidecar connections
* Support for custom DaprClient behaviors

## Configuration Options

```json
{
  "Dapr": {
    "Client": {
      "DaprApiToken": "your-api-token",  // Optional, Dapr API Token
      "HttpEndpoint": "http://localhost:3500",  // Optional, HTTP endpoint
      "GrpcEndpoint": "http://localhost:50001",  // Optional, gRPC endpoint
      "JsonSerializerOptions": {  // Optional, JSON serialization options
        "PropertyNamingPolicy": "CamelCase",
        "PropertyNameCaseInsensitive": true,
        "WriteIndented": true,
        "DefaultIgnoreCondition": "WhenWritingNull"
      },
      "GrpcChannelOptions": {  // Optional, gRPC channel options
        "Credentials": "Insecure",
        "MaxReceiveMessageSize": 1048576,
        "MaxSendMessageSize": 1048576
      }
    }
  }
}
```

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(typeof(AbpDaprModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Create a DaprClient
        context.Services.AddDaprClient();

        // Create a named DaprClient
        context.Services.AddDaprClient("__DaprClient");
        
        // Configure DaprClient options
        Configure<DaprClientFactoryOptions>(options =>
        {
            options.HttpEndpoint = "http://localhost:3500";
            options.GrpcEndpoint = "http://localhost:50001";
            options.DaprApiToken = "your-api-token";
            
            // Add DaprClient configuration actions
            options.DaprClientActions.Add(client =>
            {
                // Configure DaprClient instance
            });
            
            // Add DaprClientBuilder configuration actions
            options.DaprClientBuilderActions.Add(builder =>
            {
                // Configure DaprClientBuilder
            });
        });
    }
}
```

## Advanced Usage

### 1. Configure DaprClient

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Configure named DaprClient
    context.Services.AddDaprClient("CustomClient", builder =>
    {
        // Configure HTTP endpoint
        builder.UseHttpEndpoint("http://localhost:3500");
        
        // Configure gRPC endpoint
        builder.UseGrpcEndpoint("http://localhost:50001");
        
        // Configure API Token
        builder.UseDaprApiToken("your-api-token");
        
        // Configure JSON serialization options
        builder.UseJsonSerializerOptions(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });
        
        // Configure gRPC channel options
        builder.UseGrpcChannelOptions(new GrpcChannelOptions
        {
            MaxReceiveMessageSize = 1024 * 1024,
            MaxSendMessageSize = 1024 * 1024
        });
    });
}
```

### 2. Using DaprClient

```csharp
public class YourService
{
    private readonly IDaprClientFactory _daprClientFactory;

    public YourService(IDaprClientFactory daprClientFactory)
    {
        _daprClientFactory = daprClientFactory;
    }

    public async Task InvokeMethodAsync()
    {
        // Use default client
        var defaultClient = _daprClientFactory.CreateClient();
        
        // Use named client
        var namedClient = _daprClientFactory.CreateClient("CustomClient");
        
        // Invoke service method
        var response = await defaultClient.InvokeMethodAsync<OrderDto>(
            HttpMethod.Get,
            "order-service",  // Target service ID
            "api/orders/1",   // Method path
            new { id = 1 }    // Request parameters
        );
        
        // Publish event
        await defaultClient.PublishEventAsync(
            "pubsub",         // Pub/sub component name
            "order-created",  // Topic name
            response         // Event data
        );
        
        // Save state
        await defaultClient.SaveStateAsync(
            "statestore",    // State store component name
            "order-1",       // Key
            response        // Value
        );
        
        // Get state
        var state = await defaultClient.GetStateAsync<OrderDto>(
            "statestore",    // State store component name
            "order-1"       // Key
        );
    }
}
```

### 3. Custom DaprClient Behavior

```csharp
public class CustomDaprClientBehavior
{
    public void Configure(DaprClient client)
    {
        // Configure custom behavior
    }
}

// Register in module
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<DaprClientFactoryOptions>(options =>
    {
        options.DaprClientActions.Add(client =>
        {
            new CustomDaprClientBehavior().Configure(client);
        });
    });
}
```

## Important Notes

* DaprClient instances are thread-safe, singleton pattern is recommended
* Named DaprClients can have different configurations, suitable for scenarios requiring connections to different Dapr Sidecars
* Configuration changes require recreating the DaprClient instance to take effect
* Pay attention to performance and resource consumption when configuring gRPC channels
* JSON serialization options affect all requests using that DaprClient
* API Tokens should be managed through secure configuration management systems
* Recommended to use different named DaprClients for different microservices
* Configure appropriate timeout and retry policies in production environments

[查看中文](README.md)
