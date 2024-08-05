using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.AspNetCore.Session;

[DependsOn(
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpIdentityDomainModule))]
public class AbpIdentityAspNetCoreSessionModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            // builder.AddSignInManager<AbpIdentitySessionSignInManager>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuthenticationService, AbpIdentitySessionAuthenticationService>();
    }
}
