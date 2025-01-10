# LINGYUN.Abp.Dapr.Actors

Dapr.IActor客户端代理模块

## 功能特性

* Dapr Actors的动态代理生成
* 与ABP远程服务系统集成
* 支持Actor认证和授权
* 多租户支持
* 自动请求/响应处理
* 自定义错误处理
* 文化和语言标头支持

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

## 配置选项

```json
{
    "RemoteServices": {
        "Default": {
            "BaseUrl": "http://localhost:3500",  // 必需，Dapr HTTP端点
            "DaprApiToken": "your-api-token",    // 可选，Dapr API Token
            "RequestTimeout": "30000"            // 可选，请求超时时间（毫秒，默认：30000）
        }
    }
}
```

## 实现示例

1. Actor接口定义

```csharp
public interface ICounterActor : IActor
{
    Task<int> GetCountAsync();
    Task IncrementCountAsync();
}
```

2. Actor实现

```csharp
public class CounterActor : Actor, ICounterActor
{
    private const string CountStateName = "count";

    public CounterActor(ActorHost host) : base(host)
    {
    }

    public async Task<int> GetCountAsync()
    {
        var count = await StateManager.TryGetStateAsync<int>(CountStateName);
        return count.HasValue ? count.Value : 0;
    }

    public async Task IncrementCountAsync()
    {
        var currentCount = await GetCountAsync();
        await StateManager.SetStateAsync(CountStateName, currentCount + 1);
    }
}
```

3. 客户端使用

```csharp
public class CounterService
{
    private readonly ICounterActor _counterActor;

    public CounterService(ICounterActor counterActor)
    {
        _counterActor = counterActor;
    }

    public async Task<int> GetAndIncrementCountAsync()
    {
        var count = await _counterActor.GetCountAsync();
        await _counterActor.IncrementCountAsync();
        return count;
    }
}
```

## 注意事项

* Actor方法必须返回`Task`或`Task<T>`
* Actor方法最多只能有一个参数
* Actor实例是单线程的，一次只能处理一个请求
* Actor状态由Dapr运行时管理
* 模块自动处理：
  * 认证标头
  * 租户上下文
  * 文化信息
  * 请求超时
  * 错误处理

## 错误处理

模块为Actor调用提供自定义错误处理：
* `AbpDaprActorCallException`：当Actor方法调用失败时抛出
* `ActorMethodInvocationException`：包含有关失败的详细信息
* 错误响应包括：
  * 错误消息
  * 错误代码
  * 原始异常类型

[查看更多](README.EN.md)
