using LINGYUN.Abp.Dapr.Client.DynamicProxying;
using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.Options;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Client.Wrapper
{
    [DependsOn(typeof(AbpDaprClientModule))]
    [DependsOn(typeof(AbpWrapperModule))]
    public class AbpDaprClientWrapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDaprClientProxyOptions>(options =>
            {
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
            });
        }
    }
}
