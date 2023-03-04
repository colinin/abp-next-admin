using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Razor;

namespace LINGYUN.Abp.TextTemplating.Razor;
public class RazorTextTemplateRenderingEngine : RazorTemplateRenderingEngine, ITransientDependency
{
    protected ITemplateDefinitionStore TemplateDefinitionStore { get; }

    public RazorTextTemplateRenderingEngine(
        IServiceScopeFactory serviceScopeFactory,
        IAbpCompiledViewProvider abpCompiledViewProvider, 
        ITemplateDefinitionManager templateDefinitionManager,
        ITemplateDefinitionStore templateDefinitionStore,
        ITemplateContentProvider templateContentProvider, 
        IStringLocalizerFactory stringLocalizerFactory) 
        : base(serviceScopeFactory, abpCompiledViewProvider, templateDefinitionManager, templateContentProvider, stringLocalizerFactory)
    {
        TemplateDefinitionStore = templateDefinitionStore;
    }

    protected async override Task<string> RenderInternalAsync(string templateName, string body, Dictionary<string, object> globalContext, object model = null)
    {
        var templateDefinition = await TemplateDefinitionStore.GetAsync(templateName);

        var renderedContent = await RenderSingleTemplateAsync(
           templateDefinition,
           body,
           globalContext,
           model
       );

        if (templateDefinition.Layout != null)
        {
            renderedContent = await RenderInternalAsync(
                templateDefinition.Layout,
                renderedContent,
                globalContext
            );
        }

        return renderedContent;
    }
}
