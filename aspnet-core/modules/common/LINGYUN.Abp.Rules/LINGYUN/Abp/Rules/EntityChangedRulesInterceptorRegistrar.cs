using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Rules
{
    public static class EntityChangedRulesInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<EntityChangedRulesInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            // 拦截器的要求
            // 1、继承自IBasicRepository的仓储
            // 2、继承自INeedRule接口的实体
            return !DynamicProxyIgnoreTypes.Contains(type) &&
                   type.IsAssignableTo(typeof(IBasicRepository<>)) &&
                   type.GetGenericTypeDefinition().IsAssignableTo(typeof(INeedRule));
        }
    }
}
