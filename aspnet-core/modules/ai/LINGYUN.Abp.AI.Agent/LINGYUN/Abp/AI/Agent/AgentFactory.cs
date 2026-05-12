using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.AI;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AI.Agent;
public class AgentFactory : IAgentFactory, IScopedDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAIAgentOptions AgentOptions { get; }
    protected IChatClientFactory ChatClientFactory { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected IWorkspaceDefinitionManager WorkspaceDefinitionManager { get; }
    protected ISimpleStateCheckerManager<WorkspaceDefinition> StateCheckerManager { get; }
    public AgentFactory(
        IServiceProvider serviceProvider,
        IChatClientFactory chatClientFactory,
        IOptions<AbpAIAgentOptions> agentOptions,
        IStringLocalizerFactory stringLocalizerFactory,
        IWorkspaceDefinitionManager workspaceDefinitionManager,
        ISimpleStateCheckerManager<WorkspaceDefinition> stateCheckerManager)
    {
        AgentOptions = agentOptions.Value;
        ServiceProvider = serviceProvider;
        ChatClientFactory = chatClientFactory;
        StringLocalizerFactory = stringLocalizerFactory;
        WorkspaceDefinitionManager = workspaceDefinitionManager;
        StateCheckerManager = stateCheckerManager;
    }

    public async virtual Task<AIAgent> CreateAsync<TWorkspace>()
    {
        var workspace = WorkspaceNameAttribute.GetWorkspaceName<TWorkspace>();

        var chatClient = await ChatClientFactory.CreateAsync<TWorkspace>();

        var workspaceDefine = await WorkspaceDefinitionManager.GetOrNullAsync(workspace);

        return await CreateAgentAsync(chatClient, workspaceDefine);
    }

    public async virtual Task<AIAgent> CreateAsync(string workspace)
    {
        var workspaceDefine = await WorkspaceDefinitionManager.GetAsync(workspace);

        var chatClient = await ChatClientFactory.CreateAsync(workspace);

        return await CreateAgentAsync(chatClient, workspaceDefine);
    }

    protected async virtual Task<AIAgent> CreateAgentAsync(IChatClient chatClient, WorkspaceDefinition? workspace)
    {
        string? description = null;
        if (workspace?.Description != null)
        {
            description = workspace.Description.Localize(StringLocalizerFactory);
        }

        var tools = await GetAgentToolsAsync(workspace);

        var clientAgentOptions = new ChatClientAgentOptions
        {
            ChatOptions = new ChatOptions
            {
                Instructions = workspace?.Instructions,
                Temperature = workspace?.Temperature,
                MaxOutputTokens = workspace?.MaxOutputTokens,
                PresencePenalty = workspace?.PresencePenalty,
                FrequencyPenalty = workspace?.FrequencyPenalty,
                Tools = tools,
            },
            Name = workspace?.Name,
            Description = description,
        };

        foreach (var handlerAction in AgentOptions.AgentOptionActions)
        {
            handlerAction(workspace, clientAgentOptions);
        }

        var aiAgentBuilder = chatClient
            .CreateAIAgent(clientAgentOptions)
            .AsBuilder();

        foreach (var handlerAction in AgentOptions.AgentBuildActions)
        {
            aiAgentBuilder = await handlerAction(workspace, aiAgentBuilder);
        }

        var aiAgent = aiAgentBuilder.Build(ServiceProvider);

        return new WorkspaceAIAgent(aiAgent, workspace);
    }

    protected virtual Task<AITool[]> GetAgentToolsAsync(WorkspaceDefinition? workspace)
    {
        return Task.FromResult<AITool[]>([]);
    }
}
