using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.DataProtection;
public static class DataProtectedInterceptorRegistrar
{
    public static void RegisterIfNeeded(IOnServiceRegistredContext context)
    {
        if (ShouldIntercept(context.ImplementationType))
        {
            context.Interceptors.TryAdd<DataProtectedInterceptor>();
        }
    }

    private static bool ShouldIntercept(Type type)
    {
        return !DynamicProxyIgnoreTypes.Contains(type) &&
            (typeof(IDataProtectedEnabled).IsAssignableFrom(type) || type.IsDefined(typeof(DataProtectedAttribute), true) || AnyMethodHasAuthorizeAttribute(type));
    }

    private static bool AnyMethodHasAuthorizeAttribute(Type implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasAuthorizeAttribute);
    }

    private static bool HasAuthorizeAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(DataProtectedAttribute), true);
    }
}
