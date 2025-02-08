using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform.HttpApi.Client;

[DependsOn(
    typeof(PlatformApplicationContractModule),
    typeof(AbpHttpClientModule))]
public class PlatformHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PlatformHttpApiClientModule>();
        });

        context.Services.AddStaticHttpClientProxies(
            typeof(PlatformApplicationContractModule).Assembly,
            PlatformRemoteServiceConsts.RemoteServiceName
        );
    }
}
