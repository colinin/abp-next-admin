using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpGdprHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpGdprApplicationContractsModule).Assembly,
            GdprRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGdprHttpApiClientModule>();
        });
    }
}
