# LINGYUN.Abp.ExceptionHandling.Emailing

基于abp框架底层的**IExceptionSubscriber**的邮件通知类型

## 功能特性

* 支持异常邮件通知
* 支持自定义邮件模板
* 支持多语言邮件内容
* 支持堆栈信息发送
* 支持异常类型与接收邮箱映射

## 配置使用

使用前需要配置**AbpExceptionHandlingOptions**定义需要发送通知的异常
然后配置**AbpEmailExceptionHandlingOptions**定义具体异常类型通知方式

```csharp
    [DependsOn(
        typeof(AbpEmailingExceptionHandlingModule)
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
            // 自定义需要发送邮件通知的异常类型
            Configure<AbpEmailExceptionHandlingOptions>(options =>
            {
                // 是否发送堆栈信息
                options.SendStackTrace = true;
                // 未指定异常接收者的默认接收邮件
                options.DefaultReceiveEmail = "colin.in@foxmail.com";
                //  指定某种异常发送到哪个邮件
                options.HandReceivedException<AbpException>("colin.in@foxmail.com");
            });
        }
    }
```

## 配置项说明

* `SendStackTrace`: 是否在邮件中包含异常堆栈信息
* `DefaultTitle`: 默认邮件标题
* `DefaultContentHeader`: 默认邮件内容头部
* `DefaultContentFooter`: 默认邮件内容底部
* `DefaultReceiveEmail`: 默认异常接收邮箱
* `Handlers`: 异常类型与接收邮箱的映射字典

## 本地化资源

模块包含以下语言的本地化资源：
* en
* zh-Hans

## 相关链接

* [基础异常处理模块](../LINGYUN.Abp.ExceptionHandling/README.md)
* [异常实时通知模块](../../../modules/realtime-notifications/LINGYUN.Abp.ExceptionHandling.Notifications/README.md)

## 更多

有关更多信息和配置示例，请参阅[文档](https://github.com/colinin/abp-next-admin)。