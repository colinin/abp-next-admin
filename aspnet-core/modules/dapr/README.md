[Client](./LINGYUN.Abp.Dapr.Client/README.md) | Dapr.Actors 文档

# Dapr.Actors 集成

## 配置使用

项目设计与 **Volo.Abp.Http.Client** 类似  

### 1、接口定义

```c#

// 定义在接口上的RemoteService.Name会被作为Actor的名称注册到Dapr
[RemoteService(Name = "System")]
public interface ISystemActor : IActor
{
    Task<string> GetAsync();
}

public class SystemActorInterfaceModule : AbpModule
{

}

```

### 2、服务端

引用 LINGYUN.Abp.Dapr.Actors.AspNetCore

* 实现接口

```c#

public class SystemActor : Actor, ISystemActor 
{
    public Task<string> GetAsync() 
    {
        retuen Task.FromResult("System");
    }
}

```

* 创建模块

```c#

// 模块会自动搜索实现了IActor的服务,注册为Dapr的Actors
[DependsOn(
        typeof(AbpDaprActorsAspNetCoreModule))]
public class SystemActorServerModule : AbpModule
{

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

引用 LINGYUN.Abp.Dapr.Actors

* 配置文件 **appsettings.json**

```json

{
    "RemoteServices": {
        "Shop": {
            "BaseUrl": "http://127.0.0.1:50000"
        }
    }
}

```

* 客户端代码  

```c#

// 模块依赖
[DependsOn(
        typeof(AbpDaprActorsModule))]
public class SystemActorClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理类似于 Volo.Abp.Http.Client 模块
        context.Services.AddDaprActorProxies(
            typeof(SystemActorInterfaceModule).Assembly, // 搜索 SystemActorInterfaceModule 模块下的IActor定义
            "Shop"
        );
    }
}

// 调用方法，直接依赖注入即可
public class InvokeClass
{
    private readonly ISystemActor _systemActor;

    public InvokeClass(ISystemActor systemActor)
    {
        _systemActor = systemActor; 
    }

    public async Task InvokeAsync()
    {
        await _systemActor.GetAsync();
    }
}

```

## 其他
