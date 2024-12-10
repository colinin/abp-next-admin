# LINGYUN.Abp.Dapr.Actors.AspNetCore.Wrapper

Dapr Actors ASP.NET Core响应包装模块，用于处理Actor方法调用的响应包装和解包。

## 功能特性

* Actor响应结果自动包装/解包
* 与ABP包装系统集成
* 支持Actor方法调用的错误处理
* 支持成功/错误代码配置
* 与Dapr.Actors.AspNetCore集成
* 支持自定义响应包装格式
* 灵活的包装控制选项

## 配置使用

模块按需引用：

```csharp
[DependsOn(
    typeof(AbpDaprActorsAspNetCoreModule),
    typeof(AbpWrapperModule)
)]
public class YouProjectModule : AbpModule
{
}
```

## 配置选项

模块使用来自`LINGYUN.Abp.Wrapper`包的`AbpWrapperOptions`进行配置：

```json
{
    "Wrapper": {
        "IsEnabled": true,  // 启用/禁用响应包装
        "CodeWithSuccess": "0",  // 包装响应中的成功代码
        "HttpStatusCode": 200,  // 包装响应的默认HTTP状态码
        "WrapOnError": true,  // 是否包装错误响应
        "WrapOnSuccess": true  // 是否包装成功响应
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

## 响应格式

当启用包装时，Actor方法的响应将采用以下格式：

```json
{
    "code": "0",  // 响应代码，默认"0"表示成功
    "message": "Success",  // 响应消息
    "details": null,  // 附加详情（可选）
    "result": {  // 实际响应数据
        // ... Actor方法的返回值
    }
}
```

## 错误处理

模块自动处理Actor方法调用的错误：
* 对于包装的响应：
  * 解包响应并检查代码
  * 如果代码与`CodeWithSuccess`不匹配，抛出`AbpRemoteCallException`
  * 在异常中包含错误消息、详情和代码
  * 支持自定义错误代码映射
* 对于Actor运行时错误：
  * 自动包装为标准错误响应
  * 保留原始异常信息
  * 包含Actor相关的上下文信息

### 错误响应示例

```json
{
    "code": "ERROR_001",
    "message": "Actor方法调用失败",
    "details": "Actor 'counter' 的状态访问失败",
    "result": null
}
```

## 高级用法

### 1. 控制响应包装

可以通过HTTP头控制单个Actor调用的响应包装：

```csharp
// 在请求头中添加
var headers = new Dictionary<string, string>
{
    { "X-Abp-Wrap-Result", "true" },  // 强制启用包装
    // 或
    { "X-Abp-Dont-Wrap-Result", "true" }  // 强制禁用包装
};

// 在Actor方法中使用
public async Task<int> GetCountAsync()
{
    var context = ActorContext.GetContext();
    context.Headers.Add("X-Abp-Wrap-Result", "true");
    
    var count = await StateManager.TryGetStateAsync<int>(CountStateName);
    return count.HasValue ? count.Value : 0;
}
```

### 2. 自定义错误处理

```csharp
public class CustomActorErrorHandler : IAbpWrapperErrorHandler
{
    public Task HandleAsync(AbpWrapperErrorContext context)
    {
        if (context.Exception is ActorMethodInvocationException actorException)
        {
            // 自定义Actor错误处理逻辑
            context.Response = new WrapperResponse
            {
                Code = "ACTOR_ERROR",
                Message = actorException.Message,
                Details = actorException.ActorId
            };
        }
        return Task.CompletedTask;
    }
}
```

## 注意事项

* 响应包装可以通过以下方式控制：
  * 配置文件中的全局设置
  * HTTP头控制单个请求
  * Actor方法中的动态控制
* Actor方法的错误响应会保持原始错误结构
* 模块与ABP的远程服务错误处理系统集成
* 建议在微服务架构中统一使用响应包装
* 包装格式可以通过继承`IAbpWrapperResponseBuilder`自定义
* Actor状态操作的错误会被正确包装和处理

[查看英文](README.EN.md)
