using Dapr.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client.ClientProxying;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DynamicDaprProxyInterceptorClientProxy<TService> : ClientProxyBase<TService>
    {
        protected IOptions<AbpDaprClientProxyOptions> DaprClientProxyOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDaprClientProxyOptions>>();
        protected IDaprClientFactory DaprClientFactory => LazyServiceProvider.LazyGetRequiredService<IDaprClientFactory>();

        public virtual async Task<T> CallRequestAsync<T>(ClientProxyRequestContext requestContext)
        {
            return await RequestAsync<T>(requestContext);
        }

        public virtual async Task<HttpContent> CallRequestAsync(ClientProxyRequestContext requestContext)
        {
            return await RequestAsync(requestContext);
        }

        protected override async Task<T> RequestAsync<T>(ClientProxyRequestContext requestContext)
        {
            var response = await MakeRequestAsync(requestContext);

            var responseContent = response.Content;

            if (typeof(T) == typeof(IRemoteStreamContent) ||
                 typeof(T) == typeof(RemoteStreamContent))
            {
                /* returning a class that holds a reference to response
                 * content just to be sure that GC does not dispose of
                 * it before we finish doing our work with the stream */
                return (T)(object)new RemoteStreamContent(
                    await responseContent.ReadAsStreamAsync(),
                    responseContent.Headers?.ContentDisposition?.FileNameStar ??
                    RemoveQuotes(responseContent.Headers?.ContentDisposition?.FileName).ToString(),
                    responseContent.Headers?.ContentType?.ToString(),
                    responseContent.Headers?.ContentLength);
            }

            var stringContent = await DaprClientProxyOptions
                .Value
                .ProxyResponseContent(response, LazyServiceProvider);

            if (stringContent.IsNullOrWhiteSpace())
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)stringContent;
            }

            return JsonSerializer.Deserialize<T>(stringContent);
        }

        protected override async Task<string> GetConfiguredApiVersionAsync(ClientProxyRequestContext requestContext)
        {
            var clientConfig = DaprClientProxyOptions.Value.DaprClientProxies.GetOrDefault(requestContext.ServiceType)
                               ?? throw new AbpException($"Could not get DynamicDaprClientProxyConfig for {requestContext.ServiceType.FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            return remoteServiceConfig?.Version;
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(ClientProxyRequestContext requestContext)
        {
            var clientConfig = DaprClientProxyOptions.Value.DaprClientProxies.GetOrDefault(requestContext.ServiceType) ?? throw new AbpException($"Could not get DaprClientProxyConfig for {requestContext.ServiceType.FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            var appId = remoteServiceConfig.GetAppId();
            var apiVersion = await GetApiVersionInfoAsync(requestContext);
            var methodName = await GetUrlWithParametersAsync(requestContext, apiVersion);
            // See: https://docs.dapr.io/reference/api/service_invocation_api/#examples
            var daprClient = DaprClientFactory.CreateClient(clientConfig.RemoteServiceName);
            var requestMessage = daprClient.CreateInvokeMethodRequest(
                requestContext.Action.GetHttpMethod(),
                appId,
                methodName);
            requestMessage.Content = await ClientProxyRequestPayloadBuilder.BuildContentAsync(
                requestContext.Action,
                requestContext.Arguments,
                JsonSerializer,
                apiVersion);

            AddHeaders(requestContext.Arguments, requestContext.Action, requestMessage, apiVersion);

            if (requestContext.Action.AllowAnonymous != true)
            {
                var httpClient = HttpClientFactory.Create(AbpDaprClientModule.DaprHttpClient);

                await ClientAuthenticator.Authenticate(
                    new RemoteServiceHttpClientAuthenticateContext(
                        httpClient,
                        requestMessage,
                        remoteServiceConfig,
                        clientConfig.RemoteServiceName
                    )
                );

                // 其他库可能将授权标头写入到HttpClient中
                if (requestMessage.Headers.Authorization == null &&
                    httpClient.DefaultRequestHeaders.Authorization != null)
                {
                    requestMessage.Headers.Authorization = httpClient.DefaultRequestHeaders.Authorization;
                }
            }

            // 增加一个可配置的请求消息
            foreach (var clientRequestAction in DaprClientProxyOptions.Value.ProxyRequestActions)
            {
                clientRequestAction(appId, requestMessage);
            }

            var response = await daprClient.InvokeMethodWithResponseAsync(requestMessage, GetCancellationToken(requestContext.Arguments));

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response;
        }
    }
}
