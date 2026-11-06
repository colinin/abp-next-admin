# LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis

[简体中文](./README.md) | [English](./README.EN.md)

操作限流模块的 Redis 原子实现。四种内置算法分别由一条 Lua 脚本在 Redis 内完成“读-改-写”，不再依赖分布式锁；自定义算法状态使用 Lua CAS（比较并交换）循环更新。

## 使用

```csharp
[DependsOn(typeof(AbpOperationRateLimitingStackExchangeRedisModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

```json
{
  "OperationRateLimiting": {
    "Redis": {
      "Configuration": "127.0.0.1:6379",
      "InstanceName": "your-app:"
    }
  }
}
```

也可以直接配置选项：

```csharp
Configure<AbpOperationRateLimitingRedisOptions>(options =>
{
    options.ConfigurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379");
    options.InstanceName = "your-app:";
});
```

模块会用 `context.Services.Replace` 将默认的 `DistributedCacheOperationRateLimitingStore` 替换为 `StackExchangeRedisOperationRateLimitingStore`，上层 `IOperationRateLimitingChecker` 无需任何改动。

## 配置项

| 配置 | 说明 |
| --- | --- |
| `Configuration` | StackExchange.Redis 连接字符串，默认 `127.0.0.1:6379` |
| `ConfigurationOptions` | 高级连接选项；设置后忽略 `Configuration` |
| `InstanceName` | 所有 Redis 键的统一前缀（可选） |

## Lua 脚本

| 脚本 | 数据结构 | 说明 |
| --- | --- | --- |
| `fixed-window.lua` | String（计数） + PX | 对齐时间窗口，窗口内 `INCR`，窗口结束自动过期 |
| `sliding-window.lua` | ZSET（请求时间戳） | 按时间戳精确滑动，过期成员由 `ZREMRANGEBYSCORE` 清理 |
| `token-bucket.lua` | HASH（tokens/last） | 按周期补充令牌，返回下一次可用时间 |
| `concurrency.lua` | ZSET（租约开始时间） | 清理过期租约后占用许可；`concurrency-release.lua` 释放最早租约 |

滑动窗口与并发算法的 ZSET 成员数上限为 `maxCount`（只在未超限时写入），因此内存占用受策略上限约束。

## 注意

* 脚本只操作单个 key，兼容 Redis Cluster；如需同槽位批量操作请自行使用 hash tag。
* 自定义算法使用 `IOperationRateLimitingStore.UpdateStateAsync`，其 CAS 失败会自动重试（最多 500 次），高并发下仍能保证最终计数准确。
* 与默认存储行为一致：`maxCount: 0` 的封禁策略在进入 Redis 前直接返回，不会产生键。
