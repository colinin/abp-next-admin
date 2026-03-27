using LINGYUN.Abp.AI.Tools;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolDefinitionSerializer : IAIToolDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public AIToolDefinitionSerializer(
        IGuidGenerator guidGenerator,
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        GuidGenerator = guidGenerator;
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    public async virtual Task<AIToolDefinitionRecord[]> SerializeAsync(IEnumerable<AIToolDefinition> definitions)
    {
        var records = new List<AIToolDefinitionRecord>();
        foreach (var aiToolDef in definitions)
        {
            records.Add(await SerializeAsync(aiToolDef));
        }

        return records.ToArray();
    }

    public virtual Task<AIToolDefinitionRecord> SerializeAsync(AIToolDefinition definition)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var aiToolRecord = new AIToolDefinitionRecord(
                GuidGenerator.Create(),
                definition.Name,
                definition.Provider,
                definition.Description != null ? LocalizableStringSerializer.Serialize(definition.Description) : null,
                SerializeStateCheckers(definition.StateCheckers))
            {
                IsGlobal = definition.IsGlobal,
                IsEnabled = definition.IsEnabled,
                IsSystem = true,
            };

            foreach (var property in definition.Properties)
            {
                aiToolRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(aiToolRecord);
        }
    }

    protected virtual string? SerializeStateCheckers(List<ISimpleStateChecker<AIToolDefinition>> stateCheckers)
    {
        return StateCheckerSerializer.Serialize(stateCheckers);
    }
}
