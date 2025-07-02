using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;
public static class IdentitySessionServiceCollectionExtensions
{
    /// <summary>
    /// 允许任意会话
    /// </summary>
    /// <remarks>
    /// 某些场景下可能缓存配置不同,可不检查会话缓存
    /// </remarks>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAlwaysAllowSession(this IServiceCollection services)
    {
        services.Replace(
            ServiceDescriptor.Singleton<IIdentitySessionChecker, AllowAnonymousIdentitySessionChecker>());

        return services;
    }
}
