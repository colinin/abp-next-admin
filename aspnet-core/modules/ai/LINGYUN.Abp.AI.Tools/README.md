# LINGYUN.Abp.AI.Tools

AI 工具模块. 

参考: [AI 工具调用](https://learn.microsoft.com/zh-cn/dotnet/ai/conceptual/ai-tools)  

## 功能特性


## 模块引用

```csharp
[DependsOn(typeof(AbpAIToolsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 依赖模块

* [AbpAICoreModule](../LINGYUN.Abp.AI.Core)  

## 自定义函数

> 注意: 自定义函数遵循一个约定: 同步工具需要定义一个名为 `Invoke` 的函数; 异步工具需要定义一个名为 `InvokeAsync` 的函数, 如需使用DI容器, 需要将工具注入到容器  

```csharp

public class NowTimeTool : ITransientDependency
{
    private readonly IClock _clock;
    public NowTimeTool(IClock clock)
    {
        _clock = clock;
    }

    public object? Invoke()
    {
        return _clock.Now;
    }
}

public class NowTimeAIToolDefinitionProvider : AIToolDefinitionProvider
{
    context.Add(
            new AIToolDefinition(
                "GetNowTime",
                FunctionAIToolProvider.ProviderName)
            .WithFunction<NowTimeTool>());
}
```

## 扩展

实现 `IAIToolProvider` 接口, 以增加其他工具调用  
实现 `IDynamicAIToolDefinitionStore` 接口, 以增加动态AI工具管理  

```csharp
public class McpAIToolProvider : IAIToolProvider, ITransientDependency
{
    public virtual Task<AITool[]> CreateToolsAsync(AIToolDefinition definition)
    {
        // 你的具体实现
    }
}

[DependsOn(typeof(AbpAIToolsModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAIToolsOptions>(options =>
        {
            options.AIToolProviders.Add<McpAIToolProvider>();
        });
    }
}
```