using Elsa.Studio.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSaasUIHintHandlers(this IServiceCollection services)
    {
        return services.AddUIHintHandler<TenantPickerHandler>();
    }
}
