using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.DynamicProxying;

namespace LINGYUN.Abp.HttpClient.Wrapper
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(DynamicHttpProxyInterceptorClientProxy<>))]
    public class DynamicHttpProxyInterceptorWrapClientProxy<TService> 
        : DynamicHttpProxyInterceptorClientProxy<TService>, ITransientDependency
    {
        protected IOptions<AbpWrapperOptions> WrapperOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpWrapperOptions>>();
        
        protected override async Task<T> RequestAsync<T>(ClientProxyRequestContext requestContext)
        {
            var response = await RequestAndGetResponseAsync(requestContext);

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

            var stringContent = await responseContent.ReadAsStringAsync();

            if (stringContent.IsNullOrWhiteSpace())
            {
                return default;
            }

            // 对于包装后的结果需要处理
            if (response.Headers.Contains(AbpHttpWrapConsts.AbpWrapResult))
            {
                var wrapResult = JsonSerializer.Deserialize<WrapResult<T>>(stringContent);

                ThrowExceptionForResponse(wrapResult);

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)wrapResult.Result;
                }

                return wrapResult.Result;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)stringContent;
            }

            if (stringContent.IsNullOrWhiteSpace())
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(stringContent);
        }

        public override async Task<HttpContent> CallRequestAsync(ClientProxyRequestContext requestContext)
        {
            var response = await RequestAndGetResponseAsync(requestContext);
            // 对于包装后的结果需要处理
            if (response.Headers.Contains(AbpHttpWrapConsts.AbpWrapResult))
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                var wrapResult = JsonSerializer.Deserialize<WrapResult>(stringContent);

                ThrowExceptionForResponse(wrapResult);
            }

            return response.Content;
        }

        protected virtual void ThrowExceptionForResponse<T>(WrapResult<T> wrapResult)
        {
            if (!string.Equals(wrapResult.Code, WrapperOptions.Value.CodeWithSuccess))
            {
                var errorInfo = new RemoteServiceErrorInfo(
                    wrapResult.Message,
                    wrapResult.Details,
                    wrapResult.Code);
                throw new AbpRemoteCallException(errorInfo)
                {
                    HttpStatusCode = (int)WrapperOptions.Value.HttpStatusCode
                };
            }
        }

        protected virtual async Task<HttpResponseMessage> RequestAndGetResponseAsync(ClientProxyRequestContext requestContext)
        {
            var clientConfig = ClientOptions.Value.HttpClientProxies.GetOrDefault(requestContext.ServiceType) ?? throw new AbpException($"Could not get HttpClientProxyConfig for {requestContext.ServiceType.FullName}.");
            var remoteServiceConfig = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultAsync(clientConfig.RemoteServiceName);

            var client = HttpClientFactory.Create(clientConfig.RemoteServiceName);

            var apiVersion = await GetApiVersionInfoAsync(requestContext);
            var url = remoteServiceConfig.BaseUrl.EnsureEndsWith('/') + await GetUrlWithParametersAsync(requestContext, apiVersion);

            var requestMessage = new HttpRequestMessage(requestContext.Action.GetHttpMethod(), url)
            {
                Content = await ClientProxyRequestPayloadBuilder.BuildContentAsync(requestContext.Action, requestContext.Arguments, JsonSerializer, apiVersion)
            };

            AddHeaders(requestContext.Arguments, requestContext.Action, requestMessage, apiVersion);

            if (requestContext.Action.AllowAnonymous != true)
            {
                await ClientAuthenticator.Authenticate(
                    new RemoteServiceHttpClientAuthenticateContext(
                        client,
                        requestMessage,
                        remoteServiceConfig,
                        clientConfig.RemoteServiceName
                    )
                );
            }

            var response = await client.SendAsync(
                requestMessage,
                HttpCompletionOption.ResponseHeadersRead /*this will buffer only the headers, the content will be used as a stream*/,
                GetCancellationToken(requestContext.Arguments)
            );

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response;
        }
    }
}
