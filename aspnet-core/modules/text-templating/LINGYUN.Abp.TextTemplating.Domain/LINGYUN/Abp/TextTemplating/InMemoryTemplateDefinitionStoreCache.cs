using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;
public class InMemoryTemplateDefinitionStoreCache : ITemplateDefinitionStoreCache, ISingletonDependency
{
    public string CacheStamp { get; set; }
    public SemaphoreSlim SyncSemaphore { get; }
    public DateTime? LastCheckTime { get; set; }

    protected IDictionary<string, TemplateDefinition> TemplateDefinitions { get; }

    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public InMemoryTemplateDefinitionStoreCache(ILocalizableStringSerializer localizableStringSerializer)
    {
        LocalizableStringSerializer = localizableStringSerializer;

        SyncSemaphore = new(1, 1);
        TemplateDefinitions = new ConcurrentDictionary<string, TemplateDefinition>();
    }

    public virtual Task FillAsync(
        List<TextTemplateDefinition> templateDefinitionRecords,
        IReadOnlyList<TemplateDefinition> templateDefinitions)
    {
        TemplateDefinitions.Clear();

        foreach (var templateDefinitionRecord in templateDefinitionRecords)
        {
            var templateDefinition = new TemplateDefinition(
                templateDefinitionRecord.Name,
                templateDefinitionRecord.LocalizationResourceName,
                LocalizableStringSerializer.Deserialize(templateDefinitionRecord.DisplayName),
                templateDefinitionRecord.IsLayout,
                templateDefinitionRecord.Layout,
                templateDefinitionRecord.DefaultCultureName)
            {
                IsInlineLocalized = templateDefinitionRecord.IsInlineLocalized,
            };
            if (!templateDefinitionRecord.RenderEngine.IsNullOrWhiteSpace())
            {
                templateDefinition.WithRenderEngine(templateDefinitionRecord.RenderEngine);
            }
            foreach (var property in templateDefinitionRecord.ExtraProperties)
            {
                templateDefinition.WithProperty(property.Key, property.Value);
            }
            templateDefinition.WithProperty(nameof(TextTemplateDefinition.IsStatic), templateDefinitionRecord.IsStatic);

            TemplateDefinitions[templateDefinition.Name] = templateDefinition;
        }

        foreach (var templateDefinition in templateDefinitions)
        {
            if (TemplateDefinitions.TryGetValue(templateDefinition.Name, out var inCacheTemplate))
            {
                inCacheTemplate.WithProperty(nameof(TextTemplateDefinition.IsStatic), true);
            }
            else
            {
                templateDefinition.WithProperty(nameof(TextTemplateDefinition.IsStatic), true);
                TemplateDefinitions[templateDefinition.Name] = templateDefinition;
            }
        }

        return Task.CompletedTask;
    }

    public virtual TemplateDefinition GetOrNull(string name)
    {
        return TemplateDefinitions.GetOrDefault(name);
    }

    public virtual IReadOnlyList<TemplateDefinition> GetAll()
    {
        return TemplateDefinitions.Values.ToImmutableList();
    }
}
