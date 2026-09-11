using Elsa.Studio.Contracts;
using LINGYUN.Abp.ElsaNext.Studio.Blazor.Widgets;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAbpElsaStudioModule(this IServiceCollection services)
    {
        services.AddScoped<IWidget, WorkflowDefinitionPropertitesEditorWidget>();

        return services;
    }
}
