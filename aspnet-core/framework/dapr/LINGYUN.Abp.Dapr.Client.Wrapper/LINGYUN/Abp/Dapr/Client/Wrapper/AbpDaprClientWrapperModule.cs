using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.Options;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using LINGYUN.Abp.Dapr.Client.ClientProxying;

namespace LINGYUN.Abp.Dapr.Client.Wrapper
{
    [DependsOn(typeof(AbpDaprClientModule))]
    [DependsOn(typeof(AbpWrapperModule))]
    public class AbpDaprClientWrapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var wrapperOptions = context.Services.ExecutePreConfiguredActions<AbpWrapperOptions>();

            Configure<AbpDaprClientProxyOptions>(options =>
            {
                options.ProxyRequestActions.Add(
                    (_, request) =>
                    {
                        var wrapperHeader = wrapperOptions.IsEnabled
                            ? AbpHttpWrapConsts.AbpWrapResult
                            : AbpHttpWrapConsts.AbpDontWrapResult;

                        request.Headers.TryAddWithoutValidation(wrapperHeader, "true");
                    });

                options.OnResponse(async (response, serviceProvider) =>
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    // 包装后的响应结果需要处理
                    if (response.Headers.Contains(AbpHttpWrapConsts.AbpWrapResult))
                    {
                        var jsonSerializer = serviceProvider.LazyGetRequiredService<IJsonSerializer>();
                        var wrapperOptions = serviceProvider.LazyGetRequiredService<IOptions<AbpWrapperOptions>>().Value;
                        var wrapResult = jsonSerializer.Deserialize<WrapResult>(stringContent);

                        if (!string.Equals(wrapResult.Code, wrapperOptions.CodeWithSuccess))
                        {
                            var errorInfo = new RemoteServiceErrorInfo(
                                wrapResult.Message,
                                wrapResult.Details,
                                wrapResult.Code);

                            throw new AbpRemoteCallException(errorInfo)
                            {
                                HttpStatusCode = (int)wrapperOptions.HttpStatusCode
                            };
                        }

                        return jsonSerializer.Serialize(wrapResult.Result);
                    }

                    return stringContent;
                });
                options.OnError(async (response, serviceProvider) =>
                {
                    if (response.Headers.Contains(AbpHttpWrapConsts.AbpWrapResult))
                    {
                        try
                        {
                            var jsonSerializer = serviceProvider.LazyGetRequiredService<IJsonSerializer>();
                            var result = jsonSerializer.Deserialize<WrapResult>(
                                await response.Content.ReadAsStringAsync());

                            return new RemoteServiceErrorInfo(
                                result.Message,
                                result.Details,
                                result.Code);
                        }
                        catch
                        {
                            return new RemoteServiceErrorInfo
                            {
                                Message = response.ReasonPhrase,
                                Code = response.StatusCode.ToString()
                            };
                        }
                    }
                    return null;
                });
            });
        }
    }
}
