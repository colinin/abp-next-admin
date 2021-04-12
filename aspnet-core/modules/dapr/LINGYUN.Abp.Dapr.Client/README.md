# LINGYUN.Abp.Dapr.Client

实现了Dapr文档中的服务间调用,项目设计与Volo.Abp.Http.Client一致,通过配置文件即可无缝替代Volo.Abp.Http.Client  

配置参考 [AbpRemoteServiceOptions](https://docs.abp.io/zh-Hans/abp/latest/API/Dynamic-CSharp-API-Clients#abpremoteserviceoptions)  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理类似于 Volo.Abp.Http.Client 模块
        context.Services.AddDaprClientProxies(
            typeof(YouProjectActorInterfaceModule).Assembly, // 搜索 YouProjectActorInterfaceModule 模块下的远程服务定义
            RemoteServiceName
        );
    }
}
```
## 配置项说明

* AbpDaprClientOptions.GrpcEndpoint Dapr暴露的Grpc端点, 对应 **DaprClientBuilder.GrpcEndpoint**  
* AbpDaprClientOptions.HttpEndpoint Dapr暴露的Http端点, 对应 **DaprClientBuilder.HttpEndpoint**  
* AbpDaprClientOptions.GrpcChannelOptions 通过Grpc调用远程服务的配置项, 对应 **DaprClientBuilder.GrpcChannelOptions**  
    
* AbpDaprRemoteServiceOptions.RemoteServices 配置Dapr.AppId

```json

{
    "Dapr": {
        "Client": {
            "HttpEndpoint": "http://127.0.0.1:50000"
        }
    },
    "RemoteServices": {
        "System": {
            "AppId": "myapp"
        }
    }
}

```


## 其他
