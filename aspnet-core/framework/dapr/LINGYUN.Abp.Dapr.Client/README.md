[Actors](../README.md) | Dapr.Client 文档

# LINGYUN.Abp.Dapr.Client

实现了Dapr文档中的服务间调用，项目设计与Volo.Abp.Http.Client一致，通过配置文件即可无缝替代Volo.Abp.Http.Client。

配置参考 [AbpRemoteServiceOptions](https://docs.abp.io/zh-Hans/abp/latest/API/Dynamic-CSharp-API-Clients#abpremoteserviceoptions)

## 功能特性

* 与ABP远程服务系统集成
* 支持动态代理生成
* 支持服务发现和负载均衡
* 支持自定义请求和响应处理
* 支持错误处理和格式化
* 支持多服务端点配置
* 支持请求/响应拦截器
* 支持自定义DaprClient行为

## 配置选项

```json
{
    "RemoteServices": {
        "Default": {
            "AppId": "default-app",  // Dapr应用ID
            "BaseUrl": "http://localhost:3500",  // Dapr HTTP端点
            "HealthCheckUrl": "/health",  // 健康检查端点
            "RequestTimeout": 30000,  // 请求超时时间（毫秒）
            "RetryCount": 3,  // 重试次数
            "RetryWaitTime": 1000  // 重试等待时间（毫秒）
        },
        "System": {
            "AppId": "system-app",
            "BaseUrl": "http://localhost:50000",
            "Headers": {  // 自定义请求头
                "Tenant": "Default",
                "Culture": "zh-Hans"
            }
        }
    }
}
```

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理，类似于 Volo.Abp.Http.Client 模块
        context.Services.AddDaprClientProxies(
            typeof(YouProjectInterfaceModule).Assembly, // 搜索模块下的远程服务定义
            RemoteServiceName
        );

        // 配置代理选项
        Configure<AbpDaprClientProxyOptions>(options =>
        {
            // 配置请求拦截器
            options.ProxyRequestActions.Add((appId, request) =>
            {
                request.Headers.Add("Custom-Header", "Value");
            });

            // 配置响应处理
            options.OnResponse(async (response, serviceProvider) =>
            {
                return await response.Content.ReadAsStringAsync();
            });

            // 配置错误处理
            options.OnError(async (response, serviceProvider) =>
            {
                var error = await response.Content.ReadAsStringAsync();
                return new RemoteServiceErrorInfo
                {
                    Code = response.StatusCode.ToString(),
                    Message = error
                };
            });
        });
    }
}
```

## 实现示例

### 1. 接口定义

```csharp
// IApplicationService 实现了 IRemoteService
public interface ISystemAppService : IApplicationService
{
    Task<string> GetAsync();
    Task<SystemDto> CreateAsync(CreateSystemDto input);
    Task<List<SystemDto>> GetListAsync();
    Task DeleteAsync(string id);
}

public class SystemInterfaceModule : AbpModule
{
}
```

### 2. 服务端实现

```csharp
[DependsOn(
    typeof(SystemInterfaceModule),
    typeof(AbpAspNetCoreMvcModule)
)]
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

public class SystemAppService : ApplicationService, ISystemAppService
{
    private readonly ISystemRepository _systemRepository;

    public SystemAppService(ISystemRepository systemRepository)
    {
        _systemRepository = systemRepository;
    }

    public async Task<string> GetAsync()
    {
        return "System";
    }

    public async Task<SystemDto> CreateAsync(CreateSystemDto input)
    {
        var system = await _systemRepository.CreateAsync(
            new System
            {
                Name = input.Name,
                Description = input.Description
            }
        );
        return ObjectMapper.Map<System, SystemDto>(system);
    }

    public async Task<List<SystemDto>> GetListAsync()
    {
        var systems = await _systemRepository.GetListAsync();
        return ObjectMapper.Map<List<System>, List<SystemDto>>(systems);
    }

    public async Task DeleteAsync(string id)
    {
        await _systemRepository.DeleteAsync(id);
    }
}
```

### 3. 客户端使用

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class SystemClientModule : AbpModule
{
    private const string RemoteServiceName = "System";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册代理
        context.Services.AddDaprClientProxies(
            typeof(SystemInterfaceModule).Assembly,
            RemoteServiceName
        );

        // 配置重试策略
        context.Services.AddDaprClientBuilder(builder =>
        {
            builder.ConfigureHttpClient((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        });
    }
}

public class SystemService
{
    private readonly ISystemAppService _systemAppService;

    public SystemService(ISystemAppService systemAppService)
    {
        _systemAppService = systemAppService;
    }

    public async Task<List<SystemDto>> GetSystemsAsync()
    {
        try
        {
            return await _systemAppService.GetListAsync();
        }
        catch (AbpRemoteCallException ex)
        {
            // 处理远程调用异常
            _logger.LogError(ex, "Failed to get systems");
            throw;
        }
    }
}
```

## 高级用法

### 1. 自定义请求处理

```csharp
public class CustomRequestHandler
{
    public void Configure(HttpRequestMessage request)
    {
        request.Headers.Add("Correlation-Id", Guid.NewGuid().ToString());
        request.Headers.Add("Client-Version", "1.0.0");
    }
}

// 在模块中注册
Configure<AbpDaprClientProxyOptions>(options =>
{
    options.ProxyRequestActions.Add((appId, request) =>
    {
        new CustomRequestHandler().Configure(request);
    });
});
```

### 2. 自定义响应处理

```csharp
public class CustomResponseHandler
{
    public async Task<string> HandleAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        // 自定义响应处理逻辑
        return content;
    }
}

// 在模块中注册
Configure<AbpDaprClientProxyOptions>(options =>
{
    options.OnResponse(async (response, sp) =>
    {
        return await new CustomResponseHandler().HandleAsync(response);
    });
});
```

## 注意事项

* 远程服务接口必须继承`IRemoteService`
* 配置更改需要重新创建代理实例才能生效
* 建议配置适当的超时和重试策略
* 错误处理应该考虑网络异常和服务不可用的情况
* 在生产环境中应该启用服务发现
* 建议使用健康检查确保服务可用性
* 请求头配置应考虑安全性和身份验证需求
* 日志记录对于问题诊断很重要

[查看英文](README.EN.md)
