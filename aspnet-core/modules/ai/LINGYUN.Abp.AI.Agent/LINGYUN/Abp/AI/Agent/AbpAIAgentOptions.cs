using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.AI.Agent;
public class AbpAIAgentOptions
{
    public List<Action<WorkspaceDefinition?, AIAgentBuilder>> AgentBuildActions { get; }
    public List<Action<WorkspaceDefinition?, ChatClientAgentOptions>> AgentOptionActions { get; }
    public AbpAIAgentOptions()
    {
        AgentBuildActions = new List<Action<WorkspaceDefinition?, AIAgentBuilder>>();
        AgentOptionActions = new List<Action<WorkspaceDefinition?, ChatClientAgentOptions>>();
    }
}
