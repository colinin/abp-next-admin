using LINGYUN.Abp.AI.Workspaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AIManagement.Workspaces;

public interface IDynamicWorkspaceDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(List<Workspace> permissions);

    WorkspaceDefinition? GetWorkspaceOrNull(string name);

    IReadOnlyList<WorkspaceDefinition> GetWorkspaces();
}
