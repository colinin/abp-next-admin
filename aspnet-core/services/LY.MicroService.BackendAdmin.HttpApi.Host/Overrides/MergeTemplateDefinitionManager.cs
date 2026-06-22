using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LY.MicroService.BackendAdmin.Overrides;

[Dependency(ReplaceServices = true)]
public class MergeTemplateDefinitionManager : TemplateDefinitionManager
{
    public MergeTemplateDefinitionManager(
        IStaticTemplateDefinitionStore staticStore, 
        IDynamicTemplateDefinitionStore dynamicStore) 
        : base(staticStore, dynamicStore)
    {
    }
    public async override Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        var staticTemplates = await StaticStore.GetAllAsync();
        var dynamicTemplates = await DynamicStore.GetAllAsync();

        var mergedTemplates = new Dictionary<string, TemplateDefinition>();

        foreach (var staticTemplate in staticTemplates)
        {
            mergedTemplates[staticTemplate.Name] = staticTemplate;
        }

        foreach (var dynamicTemplate in dynamicTemplates)
        {
            if (mergedTemplates.TryGetValue(dynamicTemplate.Name, out var existingTemplate))
            {
                MergeTemplate(existingTemplate, dynamicTemplate);
            }
            else
            {
                mergedTemplates[dynamicTemplate.Name] = dynamicTemplate;
            }
        }

        return mergedTemplates.Values.ToImmutableList();
    }

    private static TemplateDefinition MergeTemplate(TemplateDefinition staticTemplate, TemplateDefinition dynamicTemplate)
    {
        var localizationResourceName = dynamicTemplate.LocalizationResourceName ?? staticTemplate.LocalizationResourceName;
        var defaultCultureName = dynamicTemplate.DefaultCultureName ?? staticTemplate.DefaultCultureName;
        var displayName = dynamicTemplate.DisplayName ?? staticTemplate.DisplayName;
        var isLayout = dynamicTemplate.IsLayout || staticTemplate.IsLayout;
        var layout = dynamicTemplate.Layout ?? staticTemplate.Layout;

        var mergedTemplate = new TemplateDefinition(
            staticTemplate.Name,
            localizationResourceName,
            displayName,
            isLayout,
            layout,
            defaultCultureName
        );

        foreach (var property in staticTemplate.Properties)
        {
            mergedTemplate.Properties[property.Key] = property.Value;
        }

        foreach (var property in dynamicTemplate.Properties)
        {
            mergedTemplate.Properties[property.Key] = property.Value;
        }

        if (!string.IsNullOrEmpty(dynamicTemplate.RenderEngine))
        {
            mergedTemplate.RenderEngine = dynamicTemplate.RenderEngine;
        }
        else if (!string.IsNullOrEmpty(staticTemplate.RenderEngine))
        {
            mergedTemplate.RenderEngine = staticTemplate.RenderEngine;
        }

        mergedTemplate.IsInlineLocalized = dynamicTemplate.IsInlineLocalized || staticTemplate.IsInlineLocalized;

        return mergedTemplate;
    }
}
