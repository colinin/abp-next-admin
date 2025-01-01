# LINGYUN.Abp.Serilog.Enrichers.Application

简体中文 | [English](./README.EN.md)

日志属性添加应用程序标识  

## 功能特性

* 为日志事件添加应用程序名称
* 支持自定义应用程序名称字段
* 支持代码配置和JSON配置两种方式
* 缓存日志事件属性以提高性能
* 与Serilog无缝集成

## 模块引用

```csharp
[DependsOn(typeof(AbpSerilogEnrichersApplicationModule))]
public class YouProjectModule : AbpModule
{
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
    AbpSerilogEnrichersConsts.ApplicationName = "demo-app";
  }
}
```

## 配置项

以下为字段常量，需要明确变更：

* AbpSerilogEnrichersConsts.ApplicationNamePropertyName - 用于自定义ApplicationName字段的名称
* AbpSerilogEnrichersConsts.ApplicationName - 在日志中标识当前应用的名称

## 使用方法

### 代码配置方式

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithApplicationName()
    // ...其他配置...
    .CreateLogger();
```

### JSON配置方式

```json
{
   "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "Enrich": [ "WithApplicationName" ]
  }
}
```

## 实现细节

该enricher为每个日志事件添加一个名为"ApplicationName"（可配置）的属性，其值为`AbpSerilogEnrichersConsts.ApplicationName`中指定的值。为了提高性能，属性会被缓存。

## 最佳实践

1. 在应用程序启动时尽早设置应用程序名称：
```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    AbpSerilogEnrichersConsts.ApplicationName = "your-app-name";
}
```

2. 为应用程序使用一致的命名约定，以便于日志过滤。

3. 考虑通过配置文件设置应用程序名称：
```json
{
  "App": {
    "Name": "your-app-name"
  }
}
```
```csharp
AbpSerilogEnrichersConsts.ApplicationName = configuration["App:Name"];
```

## 注意事项

1. 应用程序名称一旦设置就是静态的，对应用程序的所有日志条目都相同
2. enricher使用属性缓存来提高性能
3. 只有在日志事件中不存在该属性时才会添加