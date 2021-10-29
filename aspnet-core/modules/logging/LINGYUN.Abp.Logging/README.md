# LINGYUN.Abp.Logging

日志基础模块

定义 ILoggingManager 接口, 实现日志信息查询  

## 模块引用

```csharp
[DependsOn(typeof(AbpLoggingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*  AbpLoggingOptions.ApplicationPropertyName	用于自定义ApplicationName字段的名称  
*  AbpLoggingOptions.ApplicationName			在日志中标识当前应用的名称  

## appsettings.json

```json
{
  "Logging": {
    "ApplicationName": "app"
  }
}

```