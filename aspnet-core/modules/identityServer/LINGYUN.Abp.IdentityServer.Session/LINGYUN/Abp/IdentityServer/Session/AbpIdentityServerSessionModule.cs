using IdentityServer4.Validation;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.IdentityServer.Session;

[DependsOn(
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpIdentitySessionModule))]
public class AbpIdentityServerSessionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIdentityServerEventOptions>(options =>
        {
            options.EventServiceHandlers.Add<AbpIdentitySessionEventServiceHandler>();
        });

        Configure<AbpClaimsServiceOptions>(options =>
        {
            options.RequestedClaims.Add(AbpClaimTypes.SessionId);
        });

        Configure<IdentitySessionSignInOptions>(options =>
        {
            // UserLoginSuccessEvent由IdentityServer发布, 无需显式保存会话
            options.SignInSessionEnabled = false;
            // UserLoginSuccessEvent由用户发布, 需要显式注销会话
            options.SignOutSessionEnabled = true;
        });

        // 默认UserinfoEndpoint仅解密token
        // 启用此模块需要验证会话有效性
        context.Services.Replace(ServiceDescriptor.Transient<IUserInfoRequestValidator, AbpIdentitySessionUserInfoRequestValidator>());
    }
}
