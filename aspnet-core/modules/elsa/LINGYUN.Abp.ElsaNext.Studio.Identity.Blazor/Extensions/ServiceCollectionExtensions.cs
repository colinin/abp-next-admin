using Elsa.Studio.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityUIHintHandlers(this IServiceCollection services)
    {
        return services.AddUIHintHandler<IdentityPickerHandler>();
    }
}
