using System;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Features.Validation
{
    public static class FeaturesValidationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<FeaturesValidationInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return !DynamicProxyIgnoreTypes.Contains(type) &&
                   (type.IsDefined(typeof(RequiresLimitFeatureAttribute), true) ||
                    AnyMethodHasRequiresLimitFeatureAttribute(type));
        }

        private static bool AnyMethodHasRequiresLimitFeatureAttribute(Type implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasRequiresLimitFeatureAttribute);
        }

        private static bool HasRequiresLimitFeatureAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(RequiresLimitFeatureAttribute), true);
        }
    }
}
