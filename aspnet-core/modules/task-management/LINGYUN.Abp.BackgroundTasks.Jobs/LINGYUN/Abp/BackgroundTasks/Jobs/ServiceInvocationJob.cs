using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Client.Proxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class ServiceInvocationJob : IJobRunnable
{
    public const string PropertyService = "service";
    public const string PropertyMethod = "method";
    public const string PropertyCulture = "culture";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        // 获取参数列表
        var type = context.GetString(PropertyService);
        var method = context.GetString(PropertyMethod);
        var serviceType = Type.GetType(type, true);
        var serviceMethod = serviceType.GetMethod(method);
        context.TryGetString(PropertyCulture, out var culture);

        using (CultureHelper.Use(culture ?? CultureInfo.CurrentCulture.Name))
        {
            // 反射所必须的参数
            var callRequestMethod = nameof(DynamicHttpProxyInterceptorClientProxy<object>.CallRequestAsync);
            var clientProxyType = typeof(DynamicHttpProxyInterceptorClientProxy<>).MakeGenericType(serviceType);
            var clientProxy = context.GetRequiredService(clientProxyType);
            var clientProxyMethod = typeof(DynamicHttpProxyInterceptorClientProxy<>).GetMethod(callRequestMethod);

            // 调用远程服务发现端点
            var actionApiDescription = await GetActionApiDescriptionModel(context, serviceType, serviceMethod);

            // 拼接调用参数
            var invokeParameters = new Dictionary<string, object>();
            var methodParameters = serviceMethod.GetParameters();
            foreach (var parameter in methodParameters)
            {
                if (context.TryGetJobData(parameter.Name, out var value))
                {
                    invokeParameters.Add(parameter.Name, value);
                }
            }

            // 构造服务代理上下文
            var clientProxyRequestContext = new ClientProxyRequestContext(
               actionApiDescription,
               invokeParameters,
               serviceType);

            if (serviceMethod.ReturnType.GenericTypeArguments.IsNullOrEmpty())
            {
                // 直接调用
                var taskProxy = (Task)clientProxyMethod.Invoke(clientProxy, new object[] { clientProxyRequestContext });
                await taskProxy;
            }
            else
            {
                // 有返回值的调用

                var callRequestAsyncMethod = typeof(DynamicHttpProxyInterceptor<object>)
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(m => m.Name == callRequestMethod && m.IsGenericMethodDefinition);

                var returnType = serviceMethod.ReturnType.GenericTypeArguments[0];
                var result = (Task)callRequestAsyncMethod
                    .MakeGenericMethod(returnType)
                    .Invoke(this, new object[] { context });

                context.SetResult(await GetResultAsync(result, returnType));
            }
        }
    }

    protected virtual async Task<ActionApiDescriptionModel> GetActionApiDescriptionModel(
        JobRunnableContext context,
        Type serviceType,
        MethodInfo method)
    {
        var clientOptions = context.GetRequiredService<IOptions<AbpHttpClientOptions>>().Value;
        var remoteServiceConfigurationProvider = context.GetRequiredService<IRemoteServiceConfigurationProvider>();
        var proxyHttpClientFactory = context.GetRequiredService<IProxyHttpClientFactory>();
        var apiDescriptionFinder = context.GetRequiredService<IApiDescriptionFinder>();

        var clientConfig = clientOptions.HttpClientProxies.GetOrDefault(serviceType) ??
                           throw new AbpException($"Could not get DynamicHttpClientProxyConfig for {serviceType.FullName}.");
        var remoteServiceConfig = await remoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);
        var client = proxyHttpClientFactory.Create(clientConfig.RemoteServiceName);

        return await apiDescriptionFinder.FindActionAsync(
            client,
            remoteServiceConfig.BaseUrl,
            serviceType,
            method
        );
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
