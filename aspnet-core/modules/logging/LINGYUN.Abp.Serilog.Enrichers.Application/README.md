# LINGYUN.Abp.Serilog.Enrichers.Application

日志属性添加应用程序标识  

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

以下为字段常量,需要明确变更  

*  AbpSerilogEnrichersConsts.ApplicationNamePropertyName	用于自定义ApplicationName字段的名称  
*  AbpSerilogEnrichersConsts.ApplicationName				在日志中标识当前应用的名称  

## How to Use

```csharp

Log.Logger = new LoggerConfiguration()
    .Enrich.WithApplicationName()
    // ...other configuration...
    .CreateLogger();

```
**Or**

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