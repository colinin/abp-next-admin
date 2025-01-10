# LINGYUN.Abp.Features.LimitValidation.Redis.Client

基于Redis的客户端功能限制验证组件

## 功能特性

* 继承自LINGYUN.Abp.Features.LimitValidation.Redis的所有功能
* 专门针对客户端应用的功能限制验证
* 支持客户端特定的限制策略

## 配置使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpFeaturesValidationRedisClientModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 配置Redis选项（与LINGYUN.Abp.Features.LimitValidation.Redis相同）

```json
{
  "Features": {
    "Validation": {
      "Redis": {
        "Configuration": "127.0.0.1",
        "InstanceName": "YourInstanceName"
      }
    }
  }
}
```

## 使用示例

```csharp
// 限制客户端某个功能每天最多调用1000次
[RequiresLimitFeature("YourClientFeature.DailyLimit", "YourClientFeature.Interval", LimitPolicy.Days)]
public async Task YourClientMethod()
{
    // 客户端业务逻辑
}
```

[简体中文](./README.md) | [English](./README.EN.md)
