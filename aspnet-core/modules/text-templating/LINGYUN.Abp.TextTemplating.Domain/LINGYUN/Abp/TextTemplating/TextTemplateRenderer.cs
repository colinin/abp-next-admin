using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ITemplateRenderer),
    typeof(AbpTemplateRenderer))]
public class TextTemplateRenderer : AbpTemplateRenderer, ITransientDependency
{
    protected ITemplateDefinitionStore TemplateDefinitionStore { get; }
    public TextTemplateRenderer(
        IServiceScopeFactory serviceScopeFactory,
        ITemplateDefinitionManager templateDefinitionManager,
        ITemplateDefinitionStore templateDefinitionStore,
        IOptions<AbpTextTemplatingOptions> options)
        : base(serviceScopeFactory, templateDefinitionManager, options)
    {
        TemplateDefinitionStore = templateDefinitionStore;
    }

    public override async Task<string> RenderAsync(
        string templateName, 
        object model = null, 
        string cultureName = null,
        Dictionary<string, object> globalContext = null)
    {
        var templateDefinition = await TemplateDefinitionStore.GetAsync(templateName);

        var renderEngine = templateDefinition.RenderEngine;

        if (renderEngine.IsNullOrWhiteSpace())
        {
            renderEngine = Options.DefaultRenderingEngine;
        }

        var providerType = Options.RenderingEngines.GetOrDefault(renderEngine);

        if (providerType != null && typeof(ITemplateRenderingEngine).IsAssignableFrom(providerType))
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var templateRenderingEngine = (ITemplateRenderingEngine)scope.ServiceProvider.GetRequiredService(providerType);
                return await templateRenderingEngine.RenderAsync(templateName, model, cultureName, globalContext);
            }
        }

        throw new AbpException("There is no rendering engine found with template name: " + templateName);
    }
}
