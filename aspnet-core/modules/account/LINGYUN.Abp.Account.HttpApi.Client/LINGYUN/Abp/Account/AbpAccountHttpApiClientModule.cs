using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAccountHttpApiClientModule = Volo.Abp.Account.AbpAccountHttpApiClientModule;

namespace LINGYUN.Abp.Account;

[DependsOn(
    typeof(AbpAccountApplicationContractsModule),
    typeof(VoloAbpAccountHttpApiClientModule))]
public class AbpAccountHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpAccountApplicationContractsModule).Assembly,
            AccountRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountHttpApiClientModule>();
        });
    }
}
