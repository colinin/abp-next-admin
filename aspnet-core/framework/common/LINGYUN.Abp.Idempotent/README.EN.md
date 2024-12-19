# LINGYUN.Abp.Idempotent

Interface idempotency check module for preventing duplicate submissions and ensuring interface call idempotency.

## Features

* Automatic idempotency checking
* Support for custom idempotent key generation
* Flexible timeout configuration
* Distributed lock support
* Multi-language error messages
* Support for ignoring specific interfaces or methods

## Installation

```bash
dotnet add package LINGYUN.Abp.Idempotent
```

## Configuration

```csharp
[DependsOn(typeof(AbpIdempotentModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIdempotentOptions>(options =>
        {
            // Enable idempotency check globally
            options.IsEnabled = true;
            // Default 5-second timeout for each interface
            options.DefaultTimeout = 5000;
            // Idempotent token name, passed through HttpHeader
            options.IdempotentTokenName = "X-With-Idempotent-Token";
            // HTTP status code when idempotency check fails
            options.HttpStatusCode = 429;
        });
    }
}
```

## Configuration Options

* `IsEnabled` - Whether to enable idempotency checking, default: false
* `DefaultTimeout` - Default idempotency check timeout (milliseconds), default: 5000
* `IdempotentTokenName` - HTTP header name for idempotency token, default: X-With-Idempotent-Token
* `HttpStatusCode` - HTTP status code when idempotency check fails, default: 429 (Too Many Requests)

## Usage Examples

### 1. Basic Usage

```csharp
[Idempotent]
public class OrderAppService : ApplicationService
{
    public async Task<OrderDto> CreateAsync(CreateOrderDto input)
    {
        // Method will automatically perform idempotency check
        return await _orderRepository.CreateAsync(input);
    }
}
```

### 2. Custom Idempotent Key

```csharp
[Idempotent(
    iodempotentKey: "custom-key", 
    timeout: 10000,
    keyMap: new[] { "orderId", "userId" })]
public async Task<OrderDto> UpdateAsync(UpdateOrderDto input)
{
    return await _orderRepository.UpdateAsync(input);
}
```

### 3. Ignore Idempotency Check

```csharp
[IgnoreIdempotent]
public async Task<OrderDto> QueryAsync(string orderId)
{
    return await _orderRepository.GetAsync(orderId);
}
```

## Important Notes

1. By default, idempotency checking is enabled for all services inheriting from `ICreateAppService`, `IUpdateAppService`, and `IDeleteAppService`
2. You can use the `[IgnoreIdempotent]` attribute to ignore idempotency checking for specific methods
3. Idempotency checking is implemented based on distributed locks to ensure correctness in distributed environments
4. It is recommended to enable idempotency checking on all interfaces that modify data

## Links

* [中文文档](./README.md)
