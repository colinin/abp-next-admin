using LINGYUN.Abp.AI.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace LINGYUN.Abp.AIManagement.Tools;
public class DynamicAIToolDefinitionStoreInMemoryCache : IDynamicAIToolDefinitionStoreInMemoryCache, ISingletonDependency
{
    public string CacheStamp { get; set; }
    protected IDictionary<string, AIToolDefinition> AIToolDefinitions { get; }
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicAIToolDefinitionStoreInMemoryCache(
        ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;

        AIToolDefinitions = new Dictionary<string, AIToolDefinition>();
    }

    public Task FillAsync(List<AIToolDefinitionRecord> tools)
    {
        AIToolDefinitions.Clear();

        foreach (var tool in tools)
        {
            var toolDef = new AIToolDefinition(
                tool.Name,
                tool.Provider,
                !tool.Description.IsNullOrWhiteSpace() ? LocalizableStringSerializer.Deserialize(tool.Description) : null)
            {
                IsEnabled = tool.IsEnabled,
                IsGlobal = tool.IsGlobal,
            };

            if (!tool.StateCheckers.IsNullOrWhiteSpace())
            {
                var checkers = StateCheckerSerializer
                    .DeserializeArray(
                        tool.StateCheckers,
                        toolDef
                    );
                toolDef.StateCheckers.AddRange(checkers);
            }

            foreach (var property in tool.ExtraProperties)
            {
                if (property.Value != null)
                {
                    toolDef.WithProperty(property.Key, property.Value);
                }
            }

            AIToolDefinitions[tool.Name] = toolDef;
        }

        return Task.CompletedTask;
    }

    public AIToolDefinition? GetAIToolOrNull(string name)
    {
        return AIToolDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<AIToolDefinition> GetAITools()
    {
        return AIToolDefinitions.Values.ToList();
    }
}
