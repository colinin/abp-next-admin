# LINGYUN.Abp.Dapr.Actors

Dapr.IActor客户端代理  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprActorsModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理类似于 Volo.Abp.Http.Client 模块
        context.Services.AddDaprActorProxies(
            typeof(YouProjectActorInterfaceModule).Assembly, // 搜索 YouProjectActorInterfaceModule 模块下的IActor定义
            RemoteServiceName
        );
    }
}
```
## 配置项说明


## 其他
