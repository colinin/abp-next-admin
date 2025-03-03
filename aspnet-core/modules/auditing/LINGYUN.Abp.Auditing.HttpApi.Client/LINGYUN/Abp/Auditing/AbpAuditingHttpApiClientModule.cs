using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Auditing;

[DependsOn(
    typeof(AbpAuditingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpAuditingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpAuditingApplicationContractsModule).Assembly,
            AuditingRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAuditingHttpApiClientModule>();
        });
    }
}
