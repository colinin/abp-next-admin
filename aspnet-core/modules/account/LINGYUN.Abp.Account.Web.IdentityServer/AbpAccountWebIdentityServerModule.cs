using IdentityServer4.Validation;
using LINGYUN.Abp.Account.Web.IdentityServer.Handlers;
using LINGYUN.Abp.Account.Web.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
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

        //Configure<AbpIdentityServerEventOptions>(options =>
        //{
        //    options.EventServiceHandlers.Add<AbpIdentitySessionEventServiceHandler>();
        //});

        Configure<AbpClaimsServiceOptions>(options =>
        {
            options.RequestedClaims.Add(AbpClaimTypes.SessionId);
        });

        context.Services.Replace(
            ServiceDescriptor.Transient<IUserInfoRequestValidator, AbpIdentitySessionUserInfoRequestValidator>());
        context.Services.Replace(
            ServiceDescriptor.Transient<IAuthenticationService, IdentityServerSessionAuthenticationService>());
    }
}
