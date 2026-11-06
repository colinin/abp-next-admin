using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<OperationRateLimitingInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        return !DynamicProxyIgnoreTypes.Contains(type) &&
               (type.IsDefined(typeof(OperationRateLimitingAttribute), inherit: true) ||
                AnyMethodHasAttribute(type));
    }

    private static bool AnyMethodHasAttribute(Type implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(method => method.IsDefined(typeof(OperationRateLimitingAttribute), inherit: true));
    }
}
