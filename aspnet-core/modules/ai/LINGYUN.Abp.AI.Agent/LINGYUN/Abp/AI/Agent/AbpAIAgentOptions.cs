using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Agent;
public class AbpAIAgentOptions
{
    public List<Func<WorkspaceDefinition?, AIAgentBuilder, Task<AIAgentBuilder>>> AgentBuildActions { get; }
    public List<Action<WorkspaceDefinition?, ChatClientAgentOptions>> AgentOptionActions { get; }
    public AbpAIAgentOptions()
    {
        AgentOptionActions = new List<Action<WorkspaceDefinition?, ChatClientAgentOptions>>();
        AgentBuildActions = new List<Func<WorkspaceDefinition?, AIAgentBuilder, Task<AIAgentBuilder>>>();
    }
}
