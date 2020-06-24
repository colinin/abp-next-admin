# LINGYUN.Abp.ExceptionHandling

基于abp框架底层的**IExceptionSubscriber**实现二次扩展,用于自定义异常通知方式

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