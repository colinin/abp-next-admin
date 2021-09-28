[Actors](../README.md) | Dapr.Client 文档

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
            typeof(YouProjectInterfaceModule).Assembly, // 搜索 YouProjectInterfaceModule 模块下的远程服务定义
            RemoteServiceName
        );
    }
}
```


### 1、接口定义

```c#

// IApplicationService 实现了 IRemoteService
public interface ISystemAppService : IApplicationService
{
    Task<string> GetAsync();
}

public class SystemInterfaceModule : AbpModule
{

}

```

### 2、服务端

引用 Volo.Abp.AspNetCore.Mvc

* 实现接口

```c#

public class SystemAppService : ApplicationService, ISystemAppService
{
    public Task<string> GetAsync() 
    {
        retuen Task.FromResult("System");
    }
}

```

* 创建模块

```c#

[DependsOn(
        typeof(SystemInterfaceModule),
        typeof(AbpAspNetCoreMvcModule))]
public class SystemServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(SystemServerModule).Assembly);
        });
    }
}

```

* 发布到Dapr

```shell
# --app-port .net程序映射端口
# -H 对外暴露 http 监听端口
# -G 对外暴露 grpc 监听端口
dapr run --app-id myapp --app-port 5000 -H 50000 -G 40001 -- dotnet run

```

### 3、客户端

引用 LINGYUN.Abp.Dapr.Client

* 配置文件 **appsettings.json**

```json

{
    "RemoteServices": {
        "System": {
            "AppId": "myapp",
            "BaseUrl": "http://127.0.0.1:50000"
        }
    }
}

```

* 客户端代码  

```c#

// 模块依赖
[DependsOn(
        typeof(AbpDaprClientModule))]
public class SystemActorClientModule : AbpModule
{
    private const string RemoteServiceName = "System";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理类似于 Volo.Abp.Http.Client 模块
        context.Services.AddDaprClientProxies(
            typeof(SystemInterfaceModule).Assembly, // 搜索 SystemInterfaceModule 模块下的IRemoteService定义创建代理
            RemoteServiceName
        );
    }
}

// 调用方法，直接依赖注入即可
public class InvokeClass
{
    private readonly ISystemAppService _systemAppService;

    public InvokeClass(ISystemAppService systemAppService)
    {
        _systemAppService = systemAppService; 
    }

    public async Task InvokeAsync()
    {
        await _systemAppService.GetAsync();
    }
}

```


## 配置项说明
    
* AbpDaprRemoteServiceOptions.RemoteServices 配置Dapr.AppId

```json

{
    "RemoteServices": {
        "System": {
            "AppId": "myapp",
            "BaserUrl": "http://127.0.0.1:50000"
        }
    }
}

```


## 其他
