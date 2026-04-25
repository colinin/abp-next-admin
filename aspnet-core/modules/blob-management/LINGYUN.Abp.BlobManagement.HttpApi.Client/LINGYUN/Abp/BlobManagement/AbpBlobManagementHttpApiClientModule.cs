using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(typeof(AbpHttpClientModule))]
public class AbpBlobManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
               typeof(AbpBlobManagementHttpApiClientModule).Assembly,
               "BlobManagement"
           );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBlobManagementHttpApiClientModule>();
        });
    }
}
