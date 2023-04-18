# LINGYUN.Abp.Elsa.Server

Elsa.Server.Api集成, 处理elsa默认端点    


## 配置使用

```csharp

    [DependsOn(
        typeof(AbpElsaServerModule)
        )]
    public class YouProjectModule : AbpModule
    {
    }
```