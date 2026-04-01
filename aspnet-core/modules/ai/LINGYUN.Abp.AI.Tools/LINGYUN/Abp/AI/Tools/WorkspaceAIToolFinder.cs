using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AI.Tools;
public class WorkspaceAIToolFinder : IWorkspaceAIToolFinder, ITransientDependency
{
    private const string AIToolDisabledScopeKey = "Abp.AI.Tools.DisabledAITool";

    private readonly IAIToolFactory _aiToolFactory;
    private readonly IAIToolDefinitionManager _aiToolDefinitionManager;
    private readonly IAmbientScopeProvider<AIToolDisabledState> _aiToolDisabledState;

    public WorkspaceAIToolFinder(
        IAIToolFactory aiToolFactory, 
        IAIToolDefinitionManager aiToolDefinitionManager,
        IAmbientScopeProvider<AIToolDisabledState> aiToolDisabledState)
    {
        _aiToolFactory = aiToolFactory;
        _aiToolDefinitionManager = aiToolDefinitionManager;
        _aiToolDisabledState = aiToolDisabledState;
    }

    public virtual IDisposable DisableAITool()
    {
        return _aiToolDisabledState.BeginScope(AIToolDisabledScopeKey, new AIToolDisabledState(true));
    }

    public async virtual Task<AITool[]?> GetToolsAsync(WorkspaceDefinition workspace)
    {
        var useAITools = new List<AITool>();
        var useAIToolDefinitions = new List<AIToolDefinition>();
        var aiToolDefinitions = await _aiToolDefinitionManager.GetAllAsync();

        if (workspace.Tools.Count > 0)
        {
            useAIToolDefinitions.AddRange(aiToolDefinitions.Where(aiTool => workspace.Tools.Contains(aiTool.Name)));
        }

        foreach (var globalAIToolDefinition in aiToolDefinitions.Where(aiTool => aiTool.IsGlobal))
        {
            if (!useAIToolDefinitions.Any(tool => tool.Name == globalAIToolDefinition.Name))
            {
                useAIToolDefinitions.Add(globalAIToolDefinition);
            }
        }

        foreach (var aiToolDefinition in useAIToolDefinitions)
        {
            var aiTools = await _aiToolFactory.CreateTool(aiToolDefinition);
            if (aiTools.Length > 0)
            {
                useAITools.AddRange(aiTools);
            }
        }

        return useAITools.ToArray();
    }

    public virtual bool IsAIToolEnabled()
    {
        var state = _aiToolDisabledState.GetValue(AIToolDisabledScopeKey);
        return state == null || !state.IsDisabled;
    }
}
