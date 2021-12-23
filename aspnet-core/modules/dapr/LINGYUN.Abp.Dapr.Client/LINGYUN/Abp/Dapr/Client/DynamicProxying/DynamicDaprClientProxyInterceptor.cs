using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DynamicDaprClientProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static MethodInfo CallRequestAsyncMethod { get; }

        static DynamicDaprClientProxyInterceptor()
        {
            CallRequestAsyncMethod = typeof(DynamicDaprClientProxyInterceptor<TService>)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(CallRequestAsync) && m.IsGenericMethodDefinition);
        }

        public ILogger<DynamicDaprClientProxyInterceptor<TService>> Logger { get; set; }
        protected DynamicDaprProxyInterceptorClientProxy<TService> InterceptorClientProxy { get; }
        protected AbpDaprClientProxyOptions ClientProxyOptions { get; }
        protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; }
        protected IDaprApiDescriptionFinder ApiDescriptionFinder { get; }

        public DynamicDaprClientProxyInterceptor(
            DynamicDaprProxyInterceptorClientProxy<TService> interceptorClientProxy,
            IOptions<AbpDaprClientProxyOptions> clientProxyOptions,
            IRemoteServiceConfigurationProvider remoteServiceConfigurationProvider,
            IDaprApiDescriptionFinder apiDescriptionFinder)
        {
            InterceptorClientProxy = interceptorClientProxy;
            RemoteServiceConfigurationProvider = remoteServiceConfigurationProvider;
            ApiDescriptionFinder = apiDescriptionFinder;
            ClientProxyOptions = clientProxyOptions.Value;

            Logger = NullLogger<DynamicDaprClientProxyInterceptor<TService>>.Instance;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var context = new ClientProxyRequestContext(
                await GetActionApiDescriptionModel(invocation),
                invocation.ArgumentsDictionary,
                typeof(TService));

            if (invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
            {
                await InterceptorClientProxy.CallRequestAsync(context);
            }
            else
            {
                var returnType = invocation.Method.ReturnType.GenericTypeArguments[0];
                var result = (Task)CallRequestAsyncMethod
                    .MakeGenericMethod(returnType)
                    .Invoke(this, new object[] { context });

                invocation.ReturnValue = await GetResultAsync(result, returnType);
            }
        }

        protected virtual async Task<ActionApiDescriptionModel> GetActionApiDescriptionModel(IAbpMethodInvocation invocation)
        {
            var clientConfig = ClientProxyOptions.DaprClientProxies.GetOrDefault(typeof(TService)) ??
                               throw new AbpException($"Could not get DynamicDaprClientProxyConfig for {typeof(TService).FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            return await ApiDescriptionFinder.FindActionAsync(
                clientConfig.RemoteServiceName,
                remoteServiceConfig.GetAppId(),
                typeof(TService),
                invocation.Method
            );
        }

        protected virtual async Task<T> CallRequestAsync<T>(ClientProxyRequestContext context)
        {
            return await InterceptorClientProxy.CallRequestAsync<T>(context);
        }

        protected virtual async Task<object> GetResultAsync(Task task, Type resultType)
        {
            await task;
            var resultProperty = typeof(Task<>)
                .MakeGenericType(resultType)
                .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public);
            Check.NotNull(resultProperty, nameof(resultProperty));
            return resultProperty.GetValue(task);
        }
    }
}
