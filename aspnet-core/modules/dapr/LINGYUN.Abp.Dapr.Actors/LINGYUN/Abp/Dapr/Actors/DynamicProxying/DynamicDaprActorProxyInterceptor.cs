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
            var isAsyncMethod = invocation.Method.IsAsync();
            if (!isAsyncMethod)
            {
                // see: https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-actors/dotnet-actors-howto/
                // Dapr Actor文档: Actor方法的返回类型必须为Task或Task<object>
                throw new AbpException("The return type of Actor method must be Task or Task<object>");
            }
            if (invocation.Arguments.Length > 1)
            {
                // see: https://docs.dapr.io/developing-applications/sdks/dotnet/dotnet-actors/dotnet-actors-howto/
                // Dapr Actor文档: Actor方法最多可以有一个参数
                throw new AbpException("Actor method can have one argument at a maximum");
            }
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
            // 添加请求头用于传递状态
            // TODO: Actor一次只能处理一个请求,使用状态管理来传递状态的可行性?
            var httpClientHandler = new DaprHttpClientHandler();
            
            // 身份认证处理
            await ActoryProxyAuthenticator.AuthenticateAsync(
                new DaprActorProxyAuthenticateContext(
                    httpClientHandler, remoteServiceConfig, actorProxyConfig.RemoteServiceName));

            AddHeaders(httpClientHandler);

            // 代理工厂
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
            // 约定的 RemoteServiceAttribute 为Actor名称
            var remoteServiceAttr = invokeType.GetTypeInfo().GetCustomAttribute<RemoteServiceAttribute>();
            var actorType = remoteServiceAttr != null
                ? remoteServiceAttr.Name
                : invokeType.Name;
            

            try
            {
                // 创建强类型代理
                var actorProxy = proxyFactory.CreateActorProxy<TService>(actorId, actorType);
                // 远程调用
                var task = (Task)invocation.Method.Invoke(actorProxy, invocation.Arguments);
                await task;

                // 存在返回值
                if (!invocation.Method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
                {
                    // 处理返回值
                    invocation.ReturnValue = typeof(Task<>)
                        .MakeGenericType(invocation.Method.ReturnType.GenericTypeArguments[0])
                        .GetProperty(nameof(Task<object>.Result), BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(task);
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
