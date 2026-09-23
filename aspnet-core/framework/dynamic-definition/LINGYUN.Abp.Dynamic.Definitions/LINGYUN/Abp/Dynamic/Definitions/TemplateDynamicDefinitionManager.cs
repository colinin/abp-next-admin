using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Dynamic.Definitions;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ITemplateDefinitionManager),
    typeof(TemplateDynamicDefinitionManager))]
public class TemplateDynamicDefinitionManager : 
    DynamicDefinitionManager<TemplateDefinition>, 
    ITemplateDefinitionManager, 
    ISingletonDependency
{
    protected IStaticTemplateDefinitionStore StaticStore { get; }
    protected IDynamicTemplateDefinitionStore DynamicStore { get; }

    public TemplateDynamicDefinitionManager(
        IStaticTemplateDefinitionStore staticStore,
        IDynamicTemplateDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options) : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        var staticDefinitions = await StaticStore.GetAllAsync();
        var dynamicDefinitions = await DynamicStore.GetAllAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<TemplateDefinition> GetAsync(string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined Template: " + name);
    }

    public async virtual Task<TemplateDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(TemplateDefinition definition)
    {
        return definition.Name;
    }

    protected override Task<TemplateDefinition> MergeDefinitionAsync(TemplateDefinition targetDefinition, TemplateDefinition sourceDefinition)
    {
        var localizationResourceName = sourceDefinition.LocalizationResourceName ?? targetDefinition.LocalizationResourceName;
        var defaultCultureName = sourceDefinition.DefaultCultureName ?? targetDefinition.DefaultCultureName;
        var displayName = sourceDefinition.DisplayName ?? targetDefinition.DisplayName;
        var isLayout = sourceDefinition.IsLayout || targetDefinition.IsLayout;
        var layout = sourceDefinition.Layout ?? targetDefinition.Layout;

        var mergedTemplate = new TemplateDefinition(
            targetDefinition.Name,
            localizationResourceName,
            displayName,
            isLayout,
            layout,
            defaultCultureName
        );

        foreach (var property in targetDefinition.Properties)
        {
            mergedTemplate.Properties[property.Key] = property.Value;
        }

        foreach (var property in sourceDefinition.Properties)
        {
            mergedTemplate.Properties[property.Key] = property.Value;
        }

        if (!string.IsNullOrEmpty(sourceDefinition.RenderEngine))
        {
            mergedTemplate.RenderEngine = sourceDefinition.RenderEngine;
        }
        else if (!string.IsNullOrEmpty(targetDefinition.RenderEngine))
        {
            mergedTemplate.RenderEngine = targetDefinition.RenderEngine;
        }

        mergedTemplate.IsInlineLocalized = sourceDefinition.IsInlineLocalized || targetDefinition.IsInlineLocalized;

        return Task.FromResult(mergedTemplate);
    }
}
