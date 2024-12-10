# LINGYUN.Abp.DistributedLocking.Dapr

An ABP distributed locking implementation based on the Dapr distributed lock API. This module provides seamless integration with Dapr's distributed locking service, supporting cross-service and cross-instance locking capabilities.

Reference: [Dapr Distributed Lock API](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/distributed-lock-api-overview/)

## Features

* Integration with ABP distributed locking system
* Support for custom lock resource owner identification
* Configurable lock timeout duration
* Automatic lock release support
* Multiple lock storage component support
* Lock acquisition and release event notifications
* Distributed lock health check support

## Configuration Options

```json
{
    "DistributedLocking": {
        "Dapr": {
            "StoreName": "lockstore",  // Storage name defined in Dapr component
            "DefaultIdentifier": "dapr-lock-owner",  // Default lock resource owner identifier
            "DefaultTimeout": "00:00:30"  // Default lock timeout (30 seconds)
        }
    }
}
```

## Basic Usage

### 1. Module Configuration

```csharp
[DependsOn(typeof(AbpDistributedLockingDaprModule))]
public class YourProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Basic configuration
        Configure<AbpDistributedLockingDaprOptions>(options =>
        {
            options.StoreName = "redis-lock";  // Use Redis as lock storage
            options.DefaultIdentifier = "my-service";  // Custom lock owner identifier
            options.DefaultTimeout = TimeSpan.FromMinutes(1);  // Set default timeout to 1 minute
        });
    }
}
```

### 2. Basic Usage

```csharp
public class OrderService
{
    private readonly IDistributedLockProvider _lockProvider;

    public OrderService(IDistributedLockProvider lockProvider)
    {
        _lockProvider = lockProvider;
    }

    public async Task ProcessOrderAsync(string orderId)
    {
        // Try to acquire lock
        using (var handle = await _lockProvider.TryAcquireAsync($"order:{orderId}"))
        {
            if (handle != null)
            {
                try
                {
                    // Execute business logic that requires locking
                    await ProcessOrderInternalAsync(orderId);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    _logger.LogError(ex, "Error occurred while processing order");
                    throw;
                }
            }
            else
            {
                throw new ConcurrencyException("Order is being processed by another process");
            }
        }
    }
}
```

### 3. Advanced Usage

```csharp
public class InventoryService
{
    private readonly IDistributedLockProvider _lockProvider;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IDistributedLockProvider lockProvider,
        ILogger<InventoryService> logger)
    {
        _lockProvider = lockProvider;
        _logger = logger;
    }

    public async Task UpdateInventoryAsync(string productId, int quantity)
    {
        // Custom lock configuration
        var lockOptions = new DistributedLockOptions
        {
            Timeout = TimeSpan.FromSeconds(10),  // Custom timeout
            RetryDelay = TimeSpan.FromMilliseconds(100)  // Retry delay
        };

        try
        {
            using (var handle = await _lockProvider.TryAcquireAsync(
                $"inventory:{productId}",
                lockOptions))
            {
                if (handle == null)
                {
                    _logger.LogWarning("Unable to acquire inventory lock for product ID: {ProductId}", productId);
                    throw new ConcurrencyException("Unable to acquire inventory lock");
                }

                // Execute inventory update operation
                await UpdateInventoryInternalAsync(productId, quantity);
            }
        }
        catch (Exception ex) when (ex is not ConcurrencyException)
        {
            _logger.LogError(ex, "Error occurred while updating inventory");
            throw;
        }
    }
}
```

## Component Configuration

### Redis Lock Store Configuration Example

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: redis-lock  # Corresponds to StoreName configuration
spec:
  type: lock.redis
  version: v1
  metadata:
    - name: redisHost
      value: localhost:6379
    - name: redisPassword
      value: ""
    - name: enableTLS
      value: false
    - name: maxRetries
      value: 5
    - name: maxRetryBackoff
      value: 5s
```

### Consul Lock Store Configuration Example

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: consul-lock
spec:
  type: lock.consul
  version: v1
  metadata:
    - name: host
      value: localhost:8500
    - name: sessionTTL
      value: 10
    - name: scheme
      value: http
```

## Core Interfaces

### ILockOwnerFinder

Interface for providing lock resource owner identification.

```csharp
public interface ILockOwnerFinder
{
    string GetOwner();
}
```

The default implementation `LockOwnerFinder`:
1. Primarily uses the current user ID as the lock owner identifier
2. Falls back to the configured `DefaultIdentifier` if no user is logged in

### Custom Lock Owner Identifier Implementation

```csharp
public class CustomLockOwnerFinder : ILockOwnerFinder
{
    private readonly ICurrentTenant _currentTenant;

    public CustomLockOwnerFinder(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public string GetOwner()
    {
        // Use combination of tenant ID and machine name as lock owner identifier
        return $"{_currentTenant.Id ?? "host"}-{Environment.MachineName}";
    }
}

// Register custom implementation
context.Services.AddTransient<ILockOwnerFinder, CustomLockOwnerFinder>();
```

## Best Practices

1. **Set Appropriate Timeout Duration**
   - Set timeout based on expected execution time of business operations
   - Avoid setting excessively long timeouts to prevent deadlocks

2. **Proper Lock Granularity**
   - Keep lock scope as small as possible, only lock necessary resources
   - Avoid holding locks for extended periods, release promptly

3. **Exception Handling**
   - Always use locks within using blocks
   - Handle lock acquisition failures appropriately
   - Log critical lock operations

4. **Performance Optimization**
   - Use appropriate storage components
   - Configure suitable retry policies
   - Monitor lock usage

## Important Notes

* Ensure Dapr Sidecar is properly configured and running
* Distributed lock component must be correctly defined in Dapr configuration
* Set appropriate lock timeouts to avoid deadlocks
* Handle lock acquisition failures properly
* Consider performance impact in high-concurrency scenarios
* Configure health checks for lock components
* Add logging for important operations

[查看中文](README.md)
