using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Scriban;

namespace LINGYUN.Abp.TextTemplating.Scriban;

public class ScribanTextTemplateRenderingEngine : ScribanTemplateRenderingEngine, ITransientDependency
{
    protected ITemplateDefinitionStore TemplateDefinitionStore { get; }
    public ScribanTextTemplateRenderingEngine(
        ITemplateDefinitionManager templateDefinitionManager,
        ITemplateDefinitionStore templateDefinitionStore,
        ITemplateContentProvider templateContentProvider,
        IStringLocalizerFactory stringLocalizerFactory) 
        : base(templateDefinitionManager, templateContentProvider, stringLocalizerFactory)
    {
        TemplateDefinitionStore = templateDefinitionStore;
    }

    protected async override Task<string> RenderInternalAsync(string templateName, Dictionary<string, object> globalContext, object model = null)
    {
        var templateDefinition = await TemplateDefinitionStore.GetAsync(templateName);

        var renderedContent = await RenderSingleTemplateAsync(
            templateDefinition,
            globalContext,
            model
        );

        if (templateDefinition.Layout != null)
        {
            globalContext["content"] = renderedContent;
            renderedContent = await RenderInternalAsync(
                templateDefinition.Layout,
                globalContext
            );
        }

        return renderedContent;
    }
}
