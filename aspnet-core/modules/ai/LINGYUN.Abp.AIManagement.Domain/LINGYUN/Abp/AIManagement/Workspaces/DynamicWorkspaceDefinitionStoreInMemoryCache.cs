using LINGYUN.Abp.AI.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class DynamicWorkspaceDefinitionStoreInMemoryCache : IDynamicWorkspaceDefinitionStoreInMemoryCache, ISingletonDependency
{
    public string CacheStamp { get; set; }
    protected IDictionary<string, WorkspaceDefinition> WorkspaceDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicWorkspaceDefinitionStoreInMemoryCache(
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;

        WorkspaceDefinitions = new Dictionary<string, WorkspaceDefinition>();
    }

    public Task FillAsync(List<WorkspaceDefinitionRecord> workspaces)
    {
        WorkspaceDefinitions.Clear();

        foreach (var workspace in workspaces)
        {
            var workspaceDef = new WorkspaceDefinition(
                workspace.Name,
                workspace.Provider,
                workspace.ModelName,
                LocalizableStringSerializer.Deserialize(workspace.DisplayName),
                !workspace.Description.IsNullOrWhiteSpace() ? LocalizableStringSerializer.Deserialize(workspace.Description) : null,
                workspace.SystemPrompt,
                workspace.Instructions,
                workspace.Temperature,
                workspace.MaxOutputTokens,
                workspace.FrequencyPenalty,
                workspace.PresencePenalty);

            if (!workspace.ApiKey.IsNullOrWhiteSpace())
            {
                workspaceDef.WithApiKey(workspace.ApiKey);
            }
            if (!workspace.ApiBaseUrl.IsNullOrWhiteSpace())
            {
                workspaceDef.WithApiBaseUrl(workspace.ApiBaseUrl);
            }
            workspaceDef.IsEnabled = workspace.IsEnabled;

            if (!workspace.StateCheckers.IsNullOrWhiteSpace())
            {
                var checkers = StateCheckerSerializer
                    .DeserializeArray(
                        workspace.StateCheckers,
                        workspaceDef
                    );
                workspaceDef.StateCheckers.AddRange(checkers);
            }

            foreach (var property in workspace.ExtraProperties)
            {
                if (property.Value != null)
                {
                    workspaceDef.WithProperty(property.Key, property.Value);
                }
            }

            WorkspaceDefinitions[workspace.Name] = workspaceDef;
        }

        return Task.CompletedTask;
    }

    public WorkspaceDefinition? GetWorkspaceOrNull(string name)
    {
        return WorkspaceDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<WorkspaceDefinition> GetWorkspaces()
    {
        return WorkspaceDefinitions.Values.ToList();
    }
}
