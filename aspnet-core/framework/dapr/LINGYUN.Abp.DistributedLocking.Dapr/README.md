# LINGYUN.Abp.DistributedLocking.Dapr

基于Dapr分布式锁API的ABP分布式锁实现。该模块提供了与Dapr分布式锁服务的无缝集成，支持跨服务、跨实例的分布式锁定功能。

参考文档: [Dapr Distributed Lock API](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/distributed-lock-api-overview/)

## 功能特性

* 与ABP分布式锁系统集成
* 支持自定义锁资源拥有者标识
* 支持可配置的锁定超时时间
* 支持锁资源自动释放
* 支持多种锁存储组件
* 支持锁获取和释放的事件通知
* 支持分布式锁的健康检查

## 配置选项

```json
{
    "DistributedLocking": {
        "Dapr": {
            "StoreName": "lockstore",  // Dapr组件中定义的存储名称
            "DefaultIdentifier": "dapr-lock-owner",  // 默认锁资源拥有者标识
            "DefaultTimeout": "00:00:30"  // 默认锁定超时时间（30秒）
        }
    }
}
```

## 基础使用

### 1. 模块配置

```csharp
[DependsOn(typeof(AbpDistributedLockingDaprModule))]
public class YourProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 基础配置
        Configure<AbpDistributedLockingDaprOptions>(options =>
        {
            options.StoreName = "redis-lock";  // 使用Redis作为锁存储
            options.DefaultIdentifier = "my-service";  // 自定义锁拥有者标识
            options.DefaultTimeout = TimeSpan.FromMinutes(1);  // 设置默认超时时间为1分钟
        });
    }
}
```

### 2. 基本用法

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
        // 尝试获取锁
        using (var handle = await _lockProvider.TryAcquireAsync($"order:{orderId}"))
        {
            if (handle != null)
            {
                try
                {
                    // 执行需要加锁的业务逻辑
                    await ProcessOrderInternalAsync(orderId);
                }
                catch (Exception ex)
                {
                    // 处理异常
                    _logger.LogError(ex, "处理订单时发生错误");
                    throw;
                }
            }
            else
            {
                throw new ConcurrencyException("订单正在被其他进程处理");
            }
        }
    }
}
```

### 3. 高级用法

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
        // 自定义锁配置
        var lockOptions = new DistributedLockOptions
        {
            Timeout = TimeSpan.FromSeconds(10),  // 自定义超时时间
            RetryDelay = TimeSpan.FromMilliseconds(100)  // 重试延迟
        };

        try
        {
            using (var handle = await _lockProvider.TryAcquireAsync(
                $"inventory:{productId}",
                lockOptions))
            {
                if (handle == null)
                {
                    _logger.LogWarning("无法获取库存锁，产品ID: {ProductId}", productId);
                    throw new ConcurrencyException("无法获取库存锁");
                }

                // 执行库存更新操作
                await UpdateInventoryInternalAsync(productId, quantity);
            }
        }
        catch (Exception ex) when (ex is not ConcurrencyException)
        {
            _logger.LogError(ex, "更新库存时发生错误");
            throw;
        }
    }
}
```

## 组件配置

### Redis锁存储配置示例

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: redis-lock  # 对应StoreName配置
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

### Consul锁存储配置示例

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

## 核心接口

### ILockOwnerFinder

提供锁资源持有者标识的接口。

```csharp
public interface ILockOwnerFinder
{
    string GetOwner();
}
```

默认实现 `LockOwnerFinder` 会：
1. 优先使用当前用户ID作为锁拥有者标识
2. 如果用户未登录，则使用配置的 `DefaultIdentifier`

### 自定义锁拥有者标识实现

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
        // 使用租户ID和机器名称组合作为锁拥有者标识
        return $"{_currentTenant.Id ?? "host"}-{Environment.MachineName}";
    }
}

// 注册自定义实现
context.Services.AddTransient<ILockOwnerFinder, CustomLockOwnerFinder>();
```

## 最佳实践

1. **合理设置超时时间**
   - 根据业务操作的预期执行时间设置合适的超时时间
   - 避免设置过长的超时时间，以防止死锁

2. **正确的锁粒度**
   - 锁的范围应该尽可能小，只锁定必要的资源
   - 避免长时间持有锁，及时释放

3. **异常处理**
   - 始终在 using 块中使用锁
   - 妥善处理锁获取失败的情况
   - 记录关键的锁操作日志

4. **性能优化**
   - 使用合适的存储组件
   - 配置适当的重试策略
   - 监控锁的使用情况

## 注意事项

* 确保Dapr Sidecar已正确配置并运行
* 分布式锁组件需要在Dapr配置中正确定义
* 合理设置锁的超时时间，避免死锁
* 正确处理锁获取失败的情况
* 在高并发场景下注意性能影响
* 建议配置锁组件的健康检查
* 重要操作建议添加日志记录

[查看英文](README.EN.md)
