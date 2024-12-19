# LINGYUN.Abp.ExceptionHandling.Notifications

基于abp框架底层的**IExceptionSubscriber**的实时通知类型，用于将异常信息通过实时通知方式发送给用户。

## 功能特性

* 支持异常实时通知
* 支持多租户
* 支持通知模板
* 支持系统级通知
* 集成了通用通知模块

## 配置使用

使用前需要配置**AbpExceptionHandlingOptions**定义需要发送通知的异常。

```csharp
    [DependsOn(
        typeof(AbpNotificationsExceptionHandlingModule)
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

## 通知内容

异常通知包含以下信息：
* `header`: 异常通知头部信息
* `footer`: 异常通知底部信息
* `loglevel`: 日志级别
* `stackTrace`: 异常堆栈信息

## 通知名称

模块使用以下通知名称：
* `NotificationsCommonNotificationNames.ExceptionHandling`: 异常处理通知名称

## 通知模板

* 发送者: System
* 通知级别: Error
* 支持多租户: 是

## 依赖模块

* `AbpExceptionHandlingModule`: 基础异常处理模块
* `AbpNotificationsCommonModule`: 通用通知模块

## 相关链接

* [基础异常处理模块](../../../framework/common/LINGYUN.Abp.ExceptionHandling/README.md)
* [异常邮件通知模块](../../../framework/common/LINGYUN.Abp.ExceptionHandling.Emailing/README.md)

## 更多

有关更多信息和配置示例，请参阅[文档](https://github.com/colinin/abp-next-admin)。
