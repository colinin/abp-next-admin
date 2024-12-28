# LINGYUN.Abp.Features.LimitValidation.Redis

基于Redis的功能限制验证组件

## 功能特性

* 使用Redis存储和验证功能调用次数限制
* 支持Lua脚本进行原子性操作
* 支持多种限制策略（分钟、小时、天、周、月、年）
* 支持自定义Redis配置选项

## 配置使用

1. 添加模块依赖

```csharp
[DependsOn(typeof(AbpFeaturesValidationRedisModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 配置Redis选项

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

## 配置项说明

* Configuration：Redis连接字符串
* InstanceName：Redis实例名称（可选）
* ConfigurationOptions：Redis配置选项（可选，用于更详细的Redis配置）

## 使用示例

```csharp
// 限制某个方法每分钟最多调用100次
[RequiresLimitFeature("YourFeature.MethodLimit", "YourFeature.Interval", LimitPolicy.Minute)]
public async Task YourMethod()
{
    // 业务逻辑
}
```

[简体中文](./README.md) | [English](./README.EN.md)
