# LINGYUN.Abp.Dapr.Actors.AspNetCore

Dapr.Asp.NetCore的Abp框架集成,扫描程序集内部实现的Actor服务列表,批量注册为Dapr.Actors  

## 功能特性

* 自动Actor服务注册
* 与ABP依赖注入系统集成
* 通过`RemoteServiceAttribute`支持自定义Actor类型名称
* 通过`ActorRuntimeOptions`配置Actor运行时
* 自动Actor端点映射
* Actor接口验证

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprActorsAspNetCoreModule))]
public class YouProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // 配置Actor运行时选项
        PreConfigure<ActorRuntimeOptions>(options =>
        {
            options.ActorIdleTimeout = TimeSpan.FromMinutes(60);
            options.ActorScanInterval = TimeSpan.FromSeconds(30);
            options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(30);
            options.DrainRebalancedActors = true;
            options.RemindersStoragePartitions = 7;
        });
    }
}
```

## 实现示例

1. 定义Actor接口

```csharp
[RemoteService("counter")] // 可选：自定义Actor类型名称
public interface ICounterActor : IActor
{
    Task<int> GetCountAsync();
    Task IncrementCountAsync();
}
```

2. 实现Actor

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

模块将自动：
1. 检测`CounterActor`实现
2. 将其注册到Dapr.Actors
3. 配置Actor运行时
4. 映射Actor端点

## Actor运行时配置

模块通过`ActorRuntimeOptions`支持所有标准的Dapr Actor运行时配置：

```csharp
PreConfigure<ActorRuntimeOptions>(options =>
{
    // Actor超时设置
    options.ActorIdleTimeout = TimeSpan.FromMinutes(60);
    options.ActorScanInterval = TimeSpan.FromSeconds(30);
    
    // 清理设置
    options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(30);
    options.DrainRebalancedActors = true;
    
    // 提醒器设置
    options.RemindersStoragePartitions = 7;
    
    // 自定义序列化设置
    options.JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
});
```

## 重要说明

* Actor实现必须在依赖注入容器中注册
* Actor接口必须继承自`IActor`
* Actor类型名称可以使用`RemoteServiceAttribute`自定义
* 模块使用ABP的端点路由系统自动映射Actor端点
* Actor运行时选项应在`PreConfigureServices`阶段配置

## 端点映射

模块自动映射以下Actor端点：
* `/dapr/actors/{actorType}/{actorId}/method/{methodName}`
* `/dapr/actors/{actorType}/{actorId}/state`
* `/dapr/actors/{actorType}/{actorId}/reminders/{reminderName}`
* `/dapr/actors/{actorType}/{actorId}/timers/{timerName}`

[View English](README.EN.md)
