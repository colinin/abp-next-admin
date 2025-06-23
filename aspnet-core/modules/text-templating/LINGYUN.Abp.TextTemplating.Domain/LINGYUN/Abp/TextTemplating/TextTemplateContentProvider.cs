using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ITemplateContentProvider),
    typeof(TemplateContentProvider))]
public class TextTemplateContentProvider : TemplateContentProvider, ITransientDependency
{
    protected ITemplateDefinitionStore TemplateDefinitionStore { get; }

    public TextTemplateContentProvider(
        ITemplateDefinitionManager templateDefinitionManager,
        ITemplateDefinitionStore templateDefinitionStore,
        IServiceScopeFactory serviceScopeFactory, 
        IOptions<AbpTextTemplatingOptions> options)
        : base(templateDefinitionManager, serviceScopeFactory, options)
    {
        TemplateDefinitionStore = templateDefinitionStore;
    }

    public async override Task<string> GetContentOrNullAsync(
        string templateName, 
        string cultureName = null, 
        bool tryDefaults = true, 
        bool useCurrentCultureIfCultureNameIsNull = true)
    {
        var template = await TemplateDefinitionStore.GetOrNullAsync(templateName);
        if (template != null)
        {
            return await GetContentOrNullAsync(template, cultureName);
        }

        return await base.GetContentOrNullAsync(templateName, cultureName, tryDefaults, useCurrentCultureIfCultureNameIsNull);
    }
}
