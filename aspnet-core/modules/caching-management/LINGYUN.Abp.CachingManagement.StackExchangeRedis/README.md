# LINGYUN.Abp.CachingManagement.StackExchangeRedis

基于 StackExchange.Redis 的缓存管理实现模块。

## 功能

* 实现了 `ICacheManager` 接口，提供基于 Redis 的缓存管理功能：
  * 获取缓存键列表（支持前缀匹配和过滤）
  * 获取缓存值
  * 设置缓存值（支持绝对过期时间和滑动过期时间）
  * 刷新缓存
  * 删除缓存

## 安装

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## 配置使用

1. 首先需要配置 Redis 连接字符串，在 `appsettings.json` 中添加：

```json
{
  "Redis": {
    "Configuration": "127.0.0.1:6379"
  }
}
```

2. 在模块的 `ConfigureServices` 方法中添加以下代码：

```csharp
Configure<RedisCacheOptions>(options =>
{
    // 可选：配置 Redis 实例名称前缀
    options.InstanceName = "MyApp:";
});

Configure<AbpDistributedCacheOptions>(options =>
{
    // 可选：配置缓存键前缀
    options.KeyPrefix = "MyApp:";
});
```

## 多租户支持

模块内置了多租户支持，会自动根据当前租户ID来隔离缓存数据：
* 有租户时的键格式：`{InstanceName}t:{TenantId}:{KeyPrefix}:{Key}`
* 无租户时的键格式：`{InstanceName}c:{KeyPrefix}:{Key}`

## 更多

有关更多信息，请参阅以下资源：

* [应用服务实现](../LINGYUN.Abp.CachingManagement.Application/README.md)
* [应用服务契约](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.md)
* [领域层](../LINGYUN.Abp.CachingManagement.Domain/README.md)
