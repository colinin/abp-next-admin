using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Idempotent;
public static class IdempotentInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<IdempotentInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        return !DynamicProxyIgnoreTypes.Contains(type) &&
               (typeof(ICreateAppService<,>).IsAssignableFrom(type) ||
                typeof(IUpdateAppService<,>).IsAssignableFrom(type) ||
                typeof(IDeleteAppService<>).IsAssignableFrom(type) ||
                AnyMethodHasIdempotentAttribute(type));
    }

    private static bool AnyMethodHasIdempotentAttribute(Type implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasIdempotentAttribute);
    }

    private static bool HasIdempotentAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(IdempotentAttribute), true);
    }
}
