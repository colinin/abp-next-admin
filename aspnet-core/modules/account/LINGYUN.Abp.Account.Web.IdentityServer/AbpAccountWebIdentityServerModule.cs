using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAccountWebIdentityServerModule = Volo.Abp.Account.Web.AbpAccountWebIdentityServerModule;

namespace LINGYUN.Abp.Account.Web.IdentityServer;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(VoloAbpAccountWebIdentityServerModule))]
public class AbpAccountWebIdentityServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebIdentityServerModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebIdentityServerModule>();
        });
    }
}
