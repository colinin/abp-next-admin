[Actors](../README.md) | Dapr.Client Documentation

# LINGYUN.Abp.Dapr.Client

Implements service-to-service invocation as described in the Dapr documentation. The project design is consistent with Volo.Abp.Http.Client and can seamlessly replace Volo.Abp.Http.Client through configuration.

For configuration reference, see [AbpRemoteServiceOptions](https://docs.abp.io/en/abp/latest/API/Dynamic-CSharp-API-Clients#abpremoteserviceoptions)

## Features

* Integration with ABP remote service system
* Dynamic proxy generation
* Service discovery and load balancing
* Custom request and response handling
* Error handling and formatting
* Multiple service endpoint configuration
* Request/response interceptors
* Custom DaprClient behavior support

## Configuration Options

```json
{
    "RemoteServices": {
        "Default": {
            "AppId": "default-app",  // Dapr application ID
            "BaseUrl": "http://localhost:3500",  // Dapr HTTP endpoint
            "HealthCheckUrl": "/health",  // Health check endpoint
            "RequestTimeout": 30000,  // Request timeout in milliseconds
            "RetryCount": 3,  // Number of retry attempts
            "RetryWaitTime": 1000  // Retry wait time in milliseconds
        },
        "System": {
            "AppId": "system-app",
            "BaseUrl": "http://localhost:50000",
            "Headers": {  // Custom request headers
                "Tenant": "Default",
                "Culture": "en-US"
            }
        }
    }
}
```

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class YourProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register proxies similar to Volo.Abp.Http.Client module
        context.Services.AddDaprClientProxies(
            typeof(YourProjectInterfaceModule).Assembly, // Search for remote service definitions
            RemoteServiceName
        );

        // Configure proxy options
        Configure<AbpDaprClientProxyOptions>(options =>
        {
            // Configure request interceptor
            options.ProxyRequestActions.Add((appId, request) =>
            {
                request.Headers.Add("Custom-Header", "Value");
            });

            // Configure response handling
            options.OnResponse(async (response, serviceProvider) =>
            {
                return await response.Content.ReadAsStringAsync();
            });

            // Configure error handling
            options.OnError(async (response, serviceProvider) =>
            {
                var error = await response.Content.ReadAsStringAsync();
                return new RemoteServiceErrorInfo
                {
                    Code = response.StatusCode.ToString(),
                    Message = error
                };
            });
        });
    }
}
```

## Implementation Example

### 1. Interface Definition

```csharp
// IApplicationService implements IRemoteService
public interface ISystemAppService : IApplicationService
{
    Task<string> GetAsync();
    Task<SystemDto> CreateAsync(CreateSystemDto input);
    Task<List<SystemDto>> GetListAsync();
    Task DeleteAsync(string id);
}

public class SystemInterfaceModule : AbpModule
{
}
```

### 2. Server Implementation

```csharp
[DependsOn(
    typeof(SystemInterfaceModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class SystemServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(SystemServerModule).Assembly);
        });
    }
}

public class SystemAppService : ApplicationService, ISystemAppService
{
    private readonly ISystemRepository _systemRepository;

    public SystemAppService(ISystemRepository systemRepository)
    {
        _systemRepository = systemRepository;
    }

    public async Task<string> GetAsync()
    {
        return "System";
    }

    public async Task<SystemDto> CreateAsync(CreateSystemDto input)
    {
        var system = await _systemRepository.CreateAsync(
            new System
            {
                Name = input.Name,
                Description = input.Description
            }
        );
        return ObjectMapper.Map<System, SystemDto>(system);
    }

    public async Task<List<SystemDto>> GetListAsync()
    {
        var systems = await _systemRepository.GetListAsync();
        return ObjectMapper.Map<List<System>, List<SystemDto>>(systems);
    }

    public async Task DeleteAsync(string id)
    {
        await _systemRepository.DeleteAsync(id);
    }
}
```

### 3. Client Usage

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class SystemClientModule : AbpModule
{
    private const string RemoteServiceName = "System";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register proxies
        context.Services.AddDaprClientProxies(
            typeof(SystemInterfaceModule).Assembly,
            RemoteServiceName
        );

        // Configure retry policy
        context.Services.AddDaprClientBuilder(builder =>
        {
            builder.ConfigureHttpClient((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        });
    }
}

public class SystemService
{
    private readonly ISystemAppService _systemAppService;

    public SystemService(ISystemAppService systemAppService)
    {
        _systemAppService = systemAppService;
    }

    public async Task<List<SystemDto>> GetSystemsAsync()
    {
        try
        {
            return await _systemAppService.GetListAsync();
        }
        catch (AbpRemoteCallException ex)
        {
            // Handle remote call exception
            _logger.LogError(ex, "Failed to get systems");
            throw;
        }
    }
}
```

## Advanced Usage

### 1. Custom Request Handling

```csharp
public class CustomRequestHandler
{
    public void Configure(HttpRequestMessage request)
    {
        request.Headers.Add("Correlation-Id", Guid.NewGuid().ToString());
        request.Headers.Add("Client-Version", "1.0.0");
    }
}

// Register in module
Configure<AbpDaprClientProxyOptions>(options =>
{
    options.ProxyRequestActions.Add((appId, request) =>
    {
        new CustomRequestHandler().Configure(request);
    });
});
```

### 2. Custom Response Handling

```csharp
public class CustomResponseHandler
{
    public async Task<string> HandleAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        // Custom response handling logic
        return content;
    }
}

// Register in module
Configure<AbpDaprClientProxyOptions>(options =>
{
    options.OnResponse(async (response, sp) =>
    {
        return await new CustomResponseHandler().HandleAsync(response);
    });
});
```

## Important Notes

* Remote service interfaces must inherit `IRemoteService`
* Configuration changes require recreating proxy instances
* Configure appropriate timeout and retry policies
* Error handling should consider network exceptions and service unavailability
* Enable service discovery in production environments
* Use health checks to ensure service availability
* Request header configuration should consider security and authentication requirements
* Logging is important for problem diagnosis

[查看中文](README.md)
