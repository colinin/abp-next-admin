using Microsoft.AspNetCore.Identity;
using Volo.Abp.Modularity;

using VoloAbpIdentityAspNetCoreModule = Volo.Abp.Identity.AspNetCore.AbpIdentityAspNetCoreModule;

namespace LINGYUN.Abp.Identity.AspNetCore;

[DependsOn(typeof(VoloAbpIdentityAspNetCoreModule))]
public class AbpIdentityAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder.AddTokenProvider<AbpPhoneNumberRegisterTokenProvider>(AbpPhoneNumberRegisterTokenProvider.ProviderName);
        });
    }
}
