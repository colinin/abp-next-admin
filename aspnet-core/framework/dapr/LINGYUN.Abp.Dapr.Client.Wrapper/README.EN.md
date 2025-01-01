# LINGYUN.Abp.Dapr.Client.Wrapper

Dapr service-to-service invocation module that handles wrapped response result unpacking.

## Features

* Automatic response result wrapping/unwrapping
* Integration with ABP's wrapper system
* Custom error handling for wrapped responses
* Support for success/error code configuration
* HTTP status code mapping
* Support for custom response wrapper format
* Flexible wrapping control options

## Basic Usage

Module reference as needed:

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class AbpDaprClientWrapperModule : AbpModule
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

1. Service Definition

```csharp
public interface IProductService
{
    Task<ProductDto> GetAsync(string id);
    Task<List<ProductDto>> GetListAsync();
    Task<ProductDto> CreateAsync(CreateProductDto input);
}
```

2. Service Implementation

```csharp
public class ProductService : IProductService
{
    private readonly DaprClient _daprClient;

    public ProductService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<ProductDto> GetAsync(string id)
    {
        // Response wrapping is handled automatically
        return await _daprClient.InvokeMethodAsync<ProductDto>(
            "product-service",  // Target service ID
            $"api/products/{id}",  // Method path
            HttpMethod.Get
        );
    }

    public async Task<List<ProductDto>> GetListAsync()
    {
        return await _daprClient.InvokeMethodAsync<List<ProductDto>>(
            "product-service", 
            "api/products",
            HttpMethod.Get
        );
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto input)
    {
        return await _daprClient.InvokeMethodAsync<ProductDto>(
            "product-service",
            "api/products",
            HttpMethod.Post,
            input
        );
    }
}
```

## Response Format

When wrapping is enabled, the response will be in the following format:

```json
{
    "code": "0",  // Response code, "0" indicates success by default
    "message": "Success",  // Response message
    "details": null,  // Additional details (optional)
    "result": {  // Actual response data
        // ... response content
    }
}
```

## Error Handling

The module automatically handles error responses:
* For wrapped responses (with `AbpWrapResult` header):
  * Unwraps the response and checks the code
  * If code doesn't match `CodeWithSuccess`, throws `AbpRemoteCallException`
  * Includes error message, details, and code in the exception
  * Supports custom error code mapping
* For unwrapped responses:
  * Passes through the original response
  * Uses standard HTTP error handling
  * Maintains original error information

### Error Response Example

```json
{
    "code": "ERROR_001",
    "message": "Product not found",
    "details": "Product with ID '123' does not exist",
    "result": null
}
```

## Advanced Usage

### 1. Controlling Response Wrapping

Response wrapping can be controlled per request using HTTP headers:

```csharp
// Add to request headers
var headers = new Dictionary<string, string>
{
    { "X-Abp-Wrap-Result", "true" },  // Force enable wrapping
    // or
    { "X-Abp-Dont-Wrap-Result", "true" }  // Force disable wrapping
};

await _daprClient.InvokeMethodAsync<ProductDto>(
    "product-service",
    "api/products",
    HttpMethod.Get,
    null,
    headers
);
```

### 2. Custom Error Handling

```csharp
public class CustomErrorHandler : IAbpWrapperErrorHandler
{
    public Task HandleAsync(AbpWrapperErrorContext context)
    {
        // Custom error handling logic
        if (context.Response.Code == "CUSTOM_ERROR")
        {
            // Special handling
        }
        return Task.CompletedTask;
    }
}
```

## Important Notes

* Response wrapping can be controlled through:
  * Global settings in configuration
  * HTTP headers for individual requests
  * Dynamic control in code
* Error responses maintain original error structure when possible
* The module integrates with ABP's remote service error handling system
* Recommended to use response wrapping consistently in microservices architecture
* Wrapper format can be customized by implementing `IAbpWrapperResponseBuilder`

[查看中文](README.md)
