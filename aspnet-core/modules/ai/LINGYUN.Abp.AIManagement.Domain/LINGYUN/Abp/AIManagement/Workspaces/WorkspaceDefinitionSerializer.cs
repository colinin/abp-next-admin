using LINGYUN.Abp.AI.Workspaces;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceDefinitionSerializer : IWorkspaceDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public WorkspaceDefinitionSerializer(
        IGuidGenerator guidGenerator,
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        GuidGenerator = guidGenerator;
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    public async virtual Task<Workspace[]> SerializeAsync(IEnumerable<WorkspaceDefinition> definitions)
    {
        var records = new List<Workspace>();
        foreach (var workspaceDef in definitions)
        {
            records.Add(await SerializeAsync(workspaceDef));
        }

        return records.ToArray();
    }

    public virtual Task<Workspace> SerializeAsync(WorkspaceDefinition definition)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var workspace = new Workspace(
                GuidGenerator.Create(),
                definition.Name,
                definition.Provider,
                definition.ModelName,
                LocalizableStringSerializer.Serialize(definition.DisplayName)!,
                definition.Description != null ? LocalizableStringSerializer.Serialize(definition.Description) : null,
                definition.ApiKey,
                definition.ApiBaseUrl,
                definition.SystemPrompt,
                definition.Instructions,
                definition.Temperature,
                definition.MaxOutputTokens,
                definition.FrequencyPenalty,
                definition.PresencePenalty,
                SerializeStateCheckers(definition.StateCheckers));

            foreach (var property in definition.Properties)
            {
                workspace.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(workspace);
        }
    }
    protected virtual string? SerializeStateCheckers(List<ISimpleStateChecker<WorkspaceDefinition>> stateCheckers)
    {
        return StateCheckerSerializer.Serialize(stateCheckers);
    }
}
