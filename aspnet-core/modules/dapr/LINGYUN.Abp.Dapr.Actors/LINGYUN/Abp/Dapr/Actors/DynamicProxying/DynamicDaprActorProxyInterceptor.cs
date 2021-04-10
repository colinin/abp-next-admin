using Dapr.Actors;
using Dapr.Actors.Client;
using LINGYUN.Abp.Dapr.Actors.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Dapr.Actors.DynamicProxying
{
    public class DynamicDaprActorProxyInterceptor<TService> : AbpInterceptor, ITransientDependency
         where TService: IActor
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected AbpDaprActorOptions DaprActorOptions { get; }
        protected AbpDaprActorProxyOptions DaprActorProxyOptions { get; }
        protected IDaprActorProxyAuthenticator ActoryProxyAuthenticator { get; }

        public ILogger<DynamicDaprActorProxyInterceptor<TService>> Logger { get; set; }

        public DynamicDaprActorProxyInterceptor(
            IOptions<AbpDaprActorProxyOptions> daprActorProxyOptions,
            IOptionsSnapshot<AbpDaprActorOptions> daprActorOptions,
            IDaprActorProxyAuthenticator actorProxyAuthenticator,
            ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
            ActoryProxyAuthenticator = actorProxyAuthenticator;
            DaprActorProxyOptions = daprActorProxyOptions.Value;
            DaprActorOptions = daprActorOptions.Value;

            Logger = NullLogger<DynamicDaprActorProxyInterceptor<TService>>.Instance;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            await MakeRequestAsync(invocation);
        }

        private async Task MakeRequestAsync(IAbpMethodInvocation invocation)
        {
            // 获取Actor配置
            var actorProxyConfig = DaprActorProxyOptions.ActorProxies.GetOrDefault(typeof(TService)) ?? throw new AbpException($"Could not get DynamicDaprActorProxyConfig for {typeof(TService).FullName}.");
            var remoteServiceConfig = DaprActorOptions.RemoteActors.GetConfigurationOrDefault(actorProxyConfig.RemoteServiceName);

            var actorProxyOptions = new ActorProxyOptions
            {
                HttpEndpoint = remoteServiceConfig.BaseUrl
            };

            // 自定义请求处理器
            // 可添加请求头
            var httpClientHandler = new DaprHttpClientHandler();
            
            // 身份认证处理
            await ActoryProxyAuthenticator.AuthenticateAsync(
                new DaprActorProxyAuthenticateContext(
                    httpClientHandler, remoteServiceConfig, actorProxyConfig.RemoteServiceName));

            AddHeaders(httpClientHandler);

            // 构建代理服务
            var proxyFactory = new ActorProxyFactory(actorProxyOptions, (HttpMessageHandler)httpClientHandler);

            await MakeRequestAsync(invocation, proxyFactory, remoteServiceConfig);
        }

        private async Task MakeRequestAsync(
            IAbpMethodInvocation invocation,
            ActorProxyFactory proxyFactory,
            DaprActorConfiguration configuration
            )
        {
            var actorId = new ActorId(configuration.ActorId);

            var invokeType = typeof(TService);
            var remoteServiceAttr = invokeType.GetTypeInfo().GetCustomAttribute<RemoteServiceAttribute>();
            var actorType = remoteServiceAttr != null
                ? remoteServiceAttr.Name
                : invokeType.Name;
            var isAsyncMethod = invocation.Method.IsAsync();

            try
            {
                var actorProxy = proxyFactory.CreateActorProxy<TService>(actorId, actorType);
                if (isAsyncMethod)
                {
                    // 调用异步Actor
                    var task = (Task)invocation.Method.Invoke(actorProxy, invocation.Arguments);
                    await task;

                    if (!invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
                    {
                        // 处理返回值
                        invocation.ReturnValue = typeof(Task<>)
                            .MakeGenericType(invocation.Method.ReturnType.GenericTypeArguments[0])
                            .GetProperty(nameof(Task<object>.Result), BindingFlags.Public | BindingFlags.Instance)
                            .GetValue(task);
                    }
                }
                else
                {
                    // 调用同步Actor
                    invocation.ReturnValue = invocation.Method.Invoke(actorProxy, invocation.Arguments);
                }
            }
            catch (ActorMethodInvocationException amie) // 其他异常忽略交给框架处理
            {
                if (amie.InnerException != null && amie.InnerException is ActorInvokeException aie)
                {
                    // Dapr 包装了远程服务异常
                    throw new AbpDaprActorCallException(
                        new RemoteServiceErrorInfo
                        {
                            Message = aie.Message,
                            Code = aie.ActualExceptionType
                        }
                    );
                }
                throw;
            }
        }

        private void AddHeaders(DaprHttpClientHandler handler)
        {
            //TenantId
            if (CurrentTenant.Id.HasValue)
            {
                //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
                handler.AddHeader(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }
            //Culture
            //TODO: Is that the way we want? Couldn't send the culture (not ui culture)
            var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!currentCulture.IsNullOrEmpty())
            {
                handler.PreConfigure(request => request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture)));
            }
        }
    }
}
