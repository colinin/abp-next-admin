using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OssManagement;

[DependsOn(
    typeof(AbpOssManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpOssManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
               typeof(AbpOssManagementApplicationContractsModule).Assembly,
               OssManagementRemoteServiceConsts.RemoteServiceName
           );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOssManagementHttpApiClientModule>();
        });
    }
}
