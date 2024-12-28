# LINGYUN.Abp.Idempotent

接口幂等性检查模块，用于防止接口重复提交和确保接口调用的幂等性。

## 功能

* 自动幂等性检查
* 支持自定义幂等键生成
* 灵活的超时配置
* 支持分布式锁
* 支持多语言错误消息
* 支持忽略特定接口或方法

## 安装

```bash
dotnet add package LINGYUN.Abp.Idempotent
```

## 配置使用

```csharp
[DependsOn(typeof(AbpIdempotentModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIdempotentOptions>(options =>
        {
            // 全局启用幂等检查
            options.IsEnabled = true;
            // 默认每个接口提供5秒超时
            options.DefaultTimeout = 5000;
            // 幂等token名称, 通过HttpHeader传递
            options.IdempotentTokenName = "X-With-Idempotent-Token";
            // 幂等校验失败时Http响应代码
            options.HttpStatusCode = 429;
        });
    }
}
```

## 配置项说明

* `IsEnabled` - 是否启用幂等检查，默认：false
* `DefaultTimeout` - 默认幂等性检查超时时间（毫秒），默认：5000
* `IdempotentTokenName` - 幂等性Token的HTTP头名称，默认：X-With-Idempotent-Token
* `HttpStatusCode` - 幂等性检查失败时的HTTP状态码，默认：429 (Too Many Requests)

## 使用示例

### 1. 基本使用

```csharp
[Idempotent]
public class OrderAppService : ApplicationService
{
    public async Task<OrderDto> CreateAsync(CreateOrderDto input)
    {
        // 方法会自动进行幂等性检查
        return await _orderRepository.CreateAsync(input);
    }
}
```

### 2. 自定义幂等键

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

### 3. 忽略幂等性检查

```csharp
[IgnoreIdempotent]
public async Task<OrderDto> QueryAsync(string orderId)
{
    return await _orderRepository.GetAsync(orderId);
}
```

## 注意事项

1. 默认对所有继承自 `ICreateAppService`、`IUpdateAppService` 和 `IDeleteAppService` 的服务启用幂等性检查
2. 可以通过 `[IgnoreIdempotent]` 特性来忽略特定方法的幂等性检查
3. 幂等性检查基于分布式锁实现，确保在分布式环境中的正确性
4. 建议在所有修改数据的接口上启用幂等性检查

## 链接

* [English document](./README.EN.md)
