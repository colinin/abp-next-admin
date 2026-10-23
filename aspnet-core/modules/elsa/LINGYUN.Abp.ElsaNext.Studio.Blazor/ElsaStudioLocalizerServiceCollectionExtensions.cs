using Elsa.Studio.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor;

public static class ElsaStudioLocalizerServiceCollectionExtensions
{
    public static IServiceCollection AddAbpElsaStudioLocalizer(this IServiceCollection services)
    {
        services.RemoveAll<ILocalizer>();
        services.AddScoped<ILocalizer, AbpElsaStudioLocalizer>();

        return services;
    }
}
