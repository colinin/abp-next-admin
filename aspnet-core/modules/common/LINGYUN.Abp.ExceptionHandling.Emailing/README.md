# LINGYUN.Abp.ExceptionHandling.Emailing

基于abp框架底层的**IExceptionSubscriber**的邮件通知类型

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