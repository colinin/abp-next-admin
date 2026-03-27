using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;
public class FunctionAIToolProvider : IAIToolProvider, ITransientDependency
{
    public const string ProviderName = "Function";
    public string Name => ProviderName;

    protected IServiceProvider ServiceProvider { get; }
    public FunctionAIToolProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual Task<AITool[]> CreateToolsAsync(AIToolDefinition definition)
    {
        var aiToolType = definition.GetFunction();
        // 框架约定, 自定义Tool只需要定义同步方法（Invoke）或异步方法（InvokeAsync）即可
        var aiToolMethodInfo = aiToolType.GetMethod("Invoke") ?? aiToolType.GetMethod("InvokeAsync");

        Check.NotNull(aiToolMethodInfo, nameof(aiToolMethodInfo));

        string? description = null;
        if (definition.Description != null)
        {
            var localizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
            description = definition.Description.Localize(localizerFactory)?.Value;
        }

        var functionAITool = AIFunctionFactory.Create(
            method: aiToolMethodInfo,
            createInstanceFunc: (AIFunctionArguments args) => 
            {
                return ActivatorUtilities.CreateInstance(args.Services ?? ServiceProvider, aiToolType);
            },
            options: new AIFunctionFactoryOptions
            {
                Name = definition.Name,
                Description = description,
                AdditionalProperties = definition.Properties,
            });

        return Task.FromResult<AITool[]>([functionAITool]);
    }
}
