# LINGYUN.Abp.Dapr

Dapr 集成基础模块, 实现dapr文档中的命名单例DaprClient

See: https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-client/dotnet-daprclient-usage

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 创建一个DaprClient
        context.Services.AddDaprClient();

        // 创建一个具名DaprClient
        context.Services.AddDaprClient("__DaprClient");
    }
}
```

## 其他
