using Microsoft.Extensions.AI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI.Tools;
public class AIToolFactory : IAIToolFactory, ITransientDependency
{
    protected ISimpleStateCheckerManager<AIToolDefinition> StateCheckerManager { get; }
    protected IAIToolDefinitionManager AIToolDefinitionManager { get; }
    protected IAIToolProviderManager AIToolProviderManager { get; }

    public AIToolFactory(
        ISimpleStateCheckerManager<AIToolDefinition> stateCheckerManager, 
        IAIToolDefinitionManager aIToolDefinitionManager, 
        IAIToolProviderManager aIToolProviderManager)
    {
        StateCheckerManager = stateCheckerManager;
        AIToolDefinitionManager = aIToolDefinitionManager;
        AIToolProviderManager = aIToolProviderManager;
    }

    public virtual AITool CreateTool(AIToolDefinition definition)
    {
        foreach (var provider in AIToolProviderManager.Providers)
        {
            if (!string.Equals(provider.Name, definition.Provider))
            {
                continue;
            }

            return provider.CreateTool(definition);
        }

        throw new AbpException($"The AITool provider implementation named {definition.Provider} was not found!");
    }

    public async virtual Task<AITool[]> CreateAllTools()
    {
        var aiTools = new List<AITool>();
        var toolDefines = await AIToolDefinitionManager.GetAllAsync();

        foreach (var toolDefine in toolDefines)
        {
            if (await StateCheckerManager.IsEnabledAsync(toolDefine))
            {
                aiTools.Add(CreateTool(toolDefine));
            }
        }

        return aiTools.ToArray();
    }
}
