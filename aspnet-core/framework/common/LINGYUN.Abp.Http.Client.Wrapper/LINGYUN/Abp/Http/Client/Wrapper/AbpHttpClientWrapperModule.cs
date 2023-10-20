using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Http.Client.Wrapper;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpWrapperModule))]
public class AbpHttpClientWrapperModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add(
                (_, builder) =>
                {
                    builder.ConfigureHttpClient((provider, client) =>
                    {
                        var wrapperOptions = provider.GetRequiredService<IOptions<AbpWrapperOptions>>();
                        var wrapperHeader = wrapperOptions.Value.IsEnabled
                            ? AbpHttpWrapConsts.AbpWrapResult
                            : AbpHttpWrapConsts.AbpDontWrapResult;

                        client.DefaultRequestHeaders.TryAddWithoutValidation(wrapperHeader, "true");
                    });
                });
        });
    }
}
