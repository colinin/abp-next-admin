# LINGYUN.Abp.AI.Core

[Abp AI Module](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence) 扩展. 

## 功能特性


## 模块引用

```csharp
[DependsOn(typeof(AbpAICoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 依赖模块

* [AbpAIModule](https://abp.io/docs/latest/framework/infrastructure/artificial-intelligence)  
* [AbpLocalizationModule](https://abp.io/docs/latest/framework/fundamentals/localization)  

## 基本用法

> 定义系统工作区  
```csharp
[DependsOn(typeof(AbpAICoreModule))]
public class YouProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAIWorkspaceOptions>(options =>
        {
            options.Workspaces.Configure<YouWorkspace>(workspace =>
            {
                workspace.ConfigureChatClient(config =>
                {
                    config.Builder = new ChatClientBuilder(
                        sp => new OpenAIClient(
                            new ApiKeyCredential("YouApiKey"),
                            new OpenAIClientOptions
                            {
                                Endpoint = new Uri("https://api.openai.com/v1"),
                            }).GetChatClient("GPT-4").AsIChatClient());
                });

                workspace.ConfigureKernel(config =>
                {
                    config.Builder = Kernel.CreateBuilder()
                        .AddOpenAIChatClient(
                            modelId: "GPT-4",
                            openAIClient: new OpenAIClient(
                                new ApiKeyCredential("YouApiKey"),
                                new OpenAIClientOptions
                                {
                                    Endpoint = new Uri("https://api.openai.com/v1"),
                                }));
                });
            });
        });
    }
}
```

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

// 使用 IKernelFactory
Console.WriteLine("Use Microsoft.SemanticKernel:");
var kernelFactory = application.ServiceProvider.GetRequiredService<IKernelFactory>();

var kernel = await kernelFactory.CreateAsync<YouWorkspace>();
var kernelResponse = kernel.InvokePromptStreamingAsync("如何优化 C# 代码性能?");
await foreach (var item in kernelResponse)
{
    Console.Write(item);
}
Console.WriteLine();

// 使用 IKernelAccessor
var kernelWithAbp = application.ServiceProvider.GetRequiredService<IKernelAccessor<YouWorkspace>>();
var kernelWithAbpResponse = kernelWithAbp.InvokePromptStreamingAsync("如何优化 C# 代码性能?");
await foreach (var item in kernelWithAbpResponse)
{
    Console.Write(item);
}
Console.WriteLine();

await application.ShutdownAsync();

Console.WriteLine();
Console.WriteLine("AI Console completed!");

Console.ReadKey();
```

> 定义动态工作区  
```csharp
public class YouWorkspaceDefinitionProvider : WorkspaceDefinitionProvider
{
    public override void Define(IWorkspaceDefinitionContext context)
    {
        context.Add(
            new WorkspaceDefinition(
                "YouWorkspace",
                OpenAIChatClientProvider.ProviderName,
                "GPT-4",
                new FixedLocalizableString("YouWorkspace"))
            .WithApiKey("YouApiKey")
            .WithApiBaseUrl("https://api.openai.com/v1"));
    }
}

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

// Microsoft.SemanticKernel
Console.WriteLine("Use Microsoft.SemanticKernel:");
var kernelFactory = application.ServiceProvider.GetRequiredService<IKernelFactory>();

var kernel = await kernelFactory.CreateAsync("YouWorkspace");
var kernelResponse = kernel.InvokePromptStreamingAsync("如何优化 C# 代码性能?");
await foreach (var item in kernelResponse)
{
    Console.Write(item);
}
Console.WriteLine();

await application.ShutdownAsync();

Console.WriteLine();
Console.WriteLine("AI Console completed!");

Console.ReadKey();

```