using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAccountWebOpenIddictModule = Volo.Abp.Account.Web.AbpAccountWebOpenIddictModule;

namespace LINGYUN.Abp.Account.Web.OpenIddict;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(VoloAbpAccountWebOpenIddictModule))]
public class AbpAccountWebOpenIddictModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebOpenIddictModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebOpenIddictModule>();
        });
    }
}
