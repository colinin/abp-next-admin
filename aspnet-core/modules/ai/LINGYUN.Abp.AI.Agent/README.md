# LINGYUN.Abp.AI.Agent

[Abp AI Module](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence) 扩展. 

## 功能特性


## 模块引用

```csharp
[DependsOn(typeof(AbpAIAgentModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 依赖模块

* [AbpAIModule](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence)  
* [AbpLocalizationModule](https://abp.io/docs/latest/framework/fundamentals/localization)  

## 基本用法

> 兼容 `IKernelAccessor` 与 `IChatClientAccessor` 的语法.  

```csharp
using LINGYUN.Abp.AI;
using LINGYUN.Abp.AI.Agent;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Volo.Abp;

var application = await AbpApplicationFactory.CreateAsync<YouProjectModule>(options =>
{
    options.UseAutofac();
});

await application.InitializeAsync();

var chatClientAgentFactory = application.ServiceProvider.GetRequiredService<IChatClientAgentFactory>();

var agent = await chatClientAgentFactory.CreateAsync<YouWorkspace>();

var agentResponse = agent.RunStreamingAsync("解释一下线性代数");

await foreach (var item in agentResponse)
{
    Console.Write(item);
}
Console.WriteLine();

await application.ShutdownAsync();

Console.WriteLine();
Console.WriteLine("AI Console completed!");

Console.ReadKey();
```

> 支持动态工作区语法.  

```csharp
using LINGYUN.Abp.AI;
using LINGYUN.Abp.AI.Agent;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Volo.Abp;

var application = await AbpApplicationFactory.CreateAsync<YouProjectModule>(options =>
{
    options.UseAutofac();
});

await application.InitializeAsync();

var chatClientAgentFactory = application.ServiceProvider.GetRequiredService<IChatClientAgentFactory>();

var agent = await chatClientAgentFactory.CreateAsync("YouWorkspace");

var agentResponse = agent.RunStreamingAsync("解释一下线性代数");

await foreach (var item in agentResponse)
{
    Console.Write(item);
}
Console.WriteLine();

await application.ShutdownAsync();

Console.WriteLine();
Console.WriteLine("AI Console completed!");

Console.ReadKey();

```