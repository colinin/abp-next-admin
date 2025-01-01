# LINGYUN.Abp.Dapr

Dapr 集成基础模块, 实现dapr文档中的命名单例DaprClient

See: https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-client/dotnet-daprclient-usage

## 功能特性

* 支持创建默认和具名DaprClient实例
* 支持配置HTTP和gRPC端点
* 支持自定义JSON序列化选项
* 支持Dapr API Token认证
* 支持gRPC通道配置
* 支持DaprClient实例配置和构建配置的扩展
* 支持多个Dapr Sidecar连接
* 支持自定义DaprClient行为

## 配置选项

```json
{
  "Dapr": {
    "Client": {
      "DaprApiToken": "your-api-token",  // 可选，Dapr API Token
      "HttpEndpoint": "http://localhost:3500",  // 可选，HTTP端点
      "GrpcEndpoint": "http://localhost:50001",  // 可选，gRPC端点
      "JsonSerializerOptions": {  // 可选，JSON序列化选项
        "PropertyNamingPolicy": "CamelCase",
        "PropertyNameCaseInsensitive": true,
        "WriteIndented": true,
        "DefaultIgnoreCondition": "WhenWritingNull"
      },
      "GrpcChannelOptions": {  // 可选，gRPC通道选项
        "Credentials": "Insecure",
        "MaxReceiveMessageSize": 1048576,
        "MaxSendMessageSize": 1048576
      }
    }
  }
}
```

## 配置使用

模块按需引用：

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
        
        // 配置DaprClient选项
        Configure<DaprClientFactoryOptions>(options =>
        {
            options.HttpEndpoint = "http://localhost:3500";
            options.GrpcEndpoint = "http://localhost:50001";
            options.DaprApiToken = "your-api-token";
            
            // 添加DaprClient配置动作
            options.DaprClientActions.Add(client =>
            {
                // 配置DaprClient实例
            });
            
            // 添加DaprClientBuilder配置动作
            options.DaprClientBuilderActions.Add(builder =>
            {
                // 配置DaprClientBuilder
            });
        });
    }
}
```

## 高级用法

### 1. 配置DaprClient

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // 配置具名DaprClient
    context.Services.AddDaprClient("CustomClient", builder =>
    {
        // 配置HTTP端点
        builder.UseHttpEndpoint("http://localhost:3500");
        
        // 配置gRPC端点
        builder.UseGrpcEndpoint("http://localhost:50001");
        
        // 配置API Token
        builder.UseDaprApiToken("your-api-token");
        
        // 配置JSON序列化选项
        builder.UseJsonSerializerOptions(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });
        
        // 配置gRPC通道选项
        builder.UseGrpcChannelOptions(new GrpcChannelOptions
        {
            MaxReceiveMessageSize = 1024 * 1024,
            MaxSendMessageSize = 1024 * 1024
        });
    });
}
```

### 2. 使用DaprClient

```csharp
public class YourService
{
    private readonly IDaprClientFactory _daprClientFactory;

    public YourService(IDaprClientFactory daprClientFactory)
    {
        _daprClientFactory = daprClientFactory;
    }

    public async Task InvokeMethodAsync()
    {
        // 使用默认客户端
        var defaultClient = _daprClientFactory.CreateClient();
        
        // 使用具名客户端
        var namedClient = _daprClientFactory.CreateClient("CustomClient");
        
        // 调用服务方法
        var response = await defaultClient.InvokeMethodAsync<OrderDto>(
            HttpMethod.Get,
            "order-service",  // 目标服务ID
            "api/orders/1",   // 方法路径
            new { id = 1 }    // 请求参数
        );
        
        // 发布事件
        await defaultClient.PublishEventAsync(
            "pubsub",         // Pub/sub组件名称
            "order-created",  // 主题名称
            response         // 事件数据
        );
        
        // 保存状态
        await defaultClient.SaveStateAsync(
            "statestore",    // 状态存储组件名称
            "order-1",       // 键
            response        // 值
        );
        
        // 获取状态
        var state = await defaultClient.GetStateAsync<OrderDto>(
            "statestore",    // 状态存储组件名称
            "order-1"       // 键
        );
    }
}
```

### 3. 自定义DaprClient行为

```csharp
public class CustomDaprClientBehavior
{
    public void Configure(DaprClient client)
    {
        // 配置自定义行为
    }
}

// 在模块中注册
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<DaprClientFactoryOptions>(options =>
    {
        options.DaprClientActions.Add(client =>
        {
            new CustomDaprClientBehavior().Configure(client);
        });
    });
}
```

## 注意事项

* DaprClient实例是线程安全的，建议使用单例模式
* 具名DaprClient可以有不同的配置，适用于需要连接不同Dapr Sidecar的场景
* 配置更改后需要重新创建DaprClient实例才能生效
* gRPC通道配置需要注意性能和资源消耗
* JSON序列化选项会影响所有使用该DaprClient的请求
* API Token应该通过安全的配置管理系统管理
* 建议为不同的微服务使用不同的具名DaprClient
* 在生产环境中应该适当配置超时和重试策略

[查看英文](README.EN.md)
