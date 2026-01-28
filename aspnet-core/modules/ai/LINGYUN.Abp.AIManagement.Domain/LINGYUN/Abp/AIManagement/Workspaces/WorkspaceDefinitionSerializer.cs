using LINGYUN.Abp.AI.Workspaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Security.Encryption;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceDefinitionSerializer : IWorkspaceDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IStringEncryptionService StringEncryptionService { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public WorkspaceDefinitionSerializer(
        IGuidGenerator guidGenerator,
        IStringEncryptionService stringEncryptionService,
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        GuidGenerator = guidGenerator;
        StringEncryptionService = stringEncryptionService;
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    public async virtual Task<WorkspaceDefinitionRecord[]> SerializeAsync(IEnumerable<WorkspaceDefinition> definitions)
    {
        var records = new List<WorkspaceDefinitionRecord>();
        foreach (var workspaceDef in definitions)
        {
            records.Add(await SerializeAsync(workspaceDef));
        }

        return records.ToArray();
    }

    public virtual Task<WorkspaceDefinitionRecord> SerializeAsync(WorkspaceDefinition definition)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var workspace = new WorkspaceDefinitionRecord(
                GuidGenerator.Create(),
                definition.Name,
                definition.Provider,
                definition.ModelName,
                LocalizableStringSerializer.Serialize(definition.DisplayName)!,
                definition.Description != null ? LocalizableStringSerializer.Serialize(definition.Description) : null,
                definition.SystemPrompt,
                definition.Instructions,
                definition.Temperature,
                definition.MaxOutputTokens,
                definition.FrequencyPenalty,
                definition.PresencePenalty,
                SerializeStateCheckers(definition.StateCheckers));

            if (!definition.ApiKey.IsNullOrWhiteSpace())
            {
                var encryptApiKey = StringEncryptionService.Encrypt(definition.ApiKey);
                workspace.SetApiKey(encryptApiKey, definition.ApiBaseUrl);
            }

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
