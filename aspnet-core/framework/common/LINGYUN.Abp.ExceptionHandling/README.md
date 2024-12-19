# LINGYUN.Abp.ExceptionHandling

基于abp框架底层的**IExceptionSubscriber**实现二次扩展,用于自定义异常通知方式

## 功能特性

* 提供统一的异常处理和通知机制
* 支持自定义异常处理程序
* 支持异常通知筛选
* 支持与其他通知模块集成（如邮件、实时通知等）

## 配置使用

使用前只需配置**AbpExceptionHandlingOptions**定义需要发送通知的异常即可。

```csharp
    [DependsOn(
        typeof(AbpExceptionHandlingModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 自定义需要处理的异常
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                //  加入需要处理的异常类型
                options.Handlers.Add<AbpException>();
            });
        }
    }
```

## 配置项说明

* `Handlers`: 异常处理程序列表，用于定义需要处理的异常类型
* `HasNotifierError`: 检查异常是否需要发送通知

## 扩展模块

* [LINGYUN.Abp.ExceptionHandling.Emailing](../LINGYUN.Abp.ExceptionHandling.Emailing/README.md): 异常邮件通知模块
* [LINGYUN.Abp.ExceptionHandling.Notifications](../../../modules/realtime-notifications/LINGYUN.Abp.ExceptionHandling.Notifications/README.md): 异常实时通知模块

## 更多

有关更多信息和配置示例，请参阅[文档](https://github.com/colinin/abp-next-admin)。