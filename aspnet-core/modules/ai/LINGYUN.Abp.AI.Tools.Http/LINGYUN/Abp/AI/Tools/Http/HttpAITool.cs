using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;

namespace LINGYUN.Abp.AI.Tools.Http;
public class HttpAITool
{
    protected HttpAIToolInvokeContext Context { get; }

    public HttpAITool(HttpAIToolInvokeContext context)
    {
        Context = context;
    }

    public async virtual Task<object?> InvokeAsync(IDictionary<string, object?>? paramters = null)
    {
        // Abp远程服务适配
        //var remoteService = Context.ToolDefinition.GetRemoteServiceOrNull();
        //var remoteMethod = Context.ToolDefinition.GetRemoteMethodOrNull();
        //if (!remoteService.IsNullOrWhiteSpace() && !remoteMethod.IsNullOrWhiteSpace())
        //{
        //    var abpHttpClientOptions = Context.ServiceProvider.GetRequiredService<IOptions<AbpHttpClientOptions>>().Value;
        //    HttpClientProxyConfig? clientProxyConfig = null;

        //    foreach (var httpClientProxyConfig in abpHttpClientOptions.HttpClientProxies.Values)
        //    {
        //        if (httpClientProxyConfig.RemoteServiceName.Equals(remoteService))
        //        {
        //            clientProxyConfig = httpClientProxyConfig;
        //            continue;
        //        }
        //    }

        //    if (clientProxyConfig != null)
        //    {
        //        // var serviceType = clientProxyConfig.Type.GetProperty(nameof(IHttpClientProxy<object>.Service));
        //        var serviceMethod = clientProxyConfig.Type.GetMethod(remoteMethod);
        //        if (serviceMethod != null)
        //        {
        //            var httpClientProxyType = typeof(IHttpClientProxy<>).MakeGenericType(clientProxyConfig.Type);
        //            var httpClientProxy = Context.ServiceProvider.GetRequiredService(httpClientProxyType);
        //            var service = httpClientProxyType.GetProperty(nameof(IHttpClientProxy<object>.Service))!.GetValue(httpClientProxy);
        //            // TODO: 暂不支持有参远程服务调用
        //            var task = (Task)serviceMethod.Invoke(service, null)!;
        //            await task;
        //            // TODO: 必须为Task<XXX>返回结构
        //            return typeof(Task<>).MakeGenericType(serviceMethod.ReturnType.GenericTypeArguments[0])
        //                .GetProperty(nameof(Task<object>.Result), BindingFlags.Public | BindingFlags.Instance)!
        //                .GetValue(task);
        //        }
        //    }
        //}

        var options = Context.ServiceProvider.GetRequiredService<IOptions<AbpAIToolsHttpOptiions>>().Value;
        var httpClientFactory = Context.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateHttpAIToolClient();

        var requestUri = Context.ToolDefinition.GetHttpEndpoint();

        if (paramters?.Count > 0)
        {
            var isFirstParam = true;
            var urlBuilder = new StringBuilder();
            foreach (var paramter in paramters)
            {
                if (paramter.Value == null)
                {
                    continue;
                }
                urlBuilder.Append(isFirstParam ? "?" : "&");
                urlBuilder.Append(paramter.Key + $"=" + System.Net.WebUtility.UrlEncode(paramter.Value.ToString()));

                isFirstParam = false;
            }

            requestUri = requestUri.RemovePostFix("/") + urlBuilder.ToString();
        }

        var httpRequestMessage = new HttpRequestMessage(
            Context.ToolDefinition.GetHttpMethod(),
            requestUri);

        var headers = Context.ToolDefinition.GetHttpHeaders();
        foreach (var header in headers)
        {
            httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
        }

        if (Context.ToolDefinition.IsUseHttpCurrentAccessToken())
        {
            var accessTokenProvider = Context.ServiceProvider.GetRequiredService<IAbpAccessTokenProvider>();

            var token = await accessTokenProvider.GetTokenAsync();
            if (!token.IsNullOrWhiteSpace())
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        AddDefaultHeaders(httpRequestMessage);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (options.HttpResponseActions.TryGetValue(Context.ToolDefinition.Name, out var customHandler))
        {
            return await customHandler(Context.ServiceProvider, httpResponseMessage);
        }

        return await httpResponseMessage.Content.ReadAsStringAsync();
    }

    protected virtual void AddDefaultHeaders(HttpRequestMessage requestMessage)
    {
        //CorrelationId
        var correlationIdProvider = Context.ServiceProvider.GetRequiredService<ICorrelationIdProvider>();
        var correlationId = correlationIdProvider.Get();
        if (correlationId != null)
        {
            var correlationIdOptions = Context.ServiceProvider.GetRequiredService<IOptions<AbpCorrelationIdOptions>>();
            requestMessage.Headers.Add(correlationIdOptions.Value.HttpHeaderName, correlationId);
        }

        //TenantId
        var currentTenant = Context.ServiceProvider.GetRequiredService<ICurrentTenant>();
        if (currentTenant.Id.HasValue)
        {
            //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
            requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, currentTenant.Id.Value.ToString());
        }

        //Culture
        //TODO: Is that the way we want? Couldn't send the culture (not ui culture)
        var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if (!currentCulture.IsNullOrEmpty())
        {
            requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture));
        }

        //X-Requested-With
        requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");

        //Timezone
        var currentTimezoneProvider = Context.ServiceProvider.GetRequiredService<ICurrentTimezoneProvider>();
        if (!currentTimezoneProvider.TimeZone.IsNullOrWhiteSpace())
        {
            requestMessage.Headers.Add(TimeZoneConsts.DefaultTimeZoneKey, currentTimezoneProvider.TimeZone);
        }
    }
}
