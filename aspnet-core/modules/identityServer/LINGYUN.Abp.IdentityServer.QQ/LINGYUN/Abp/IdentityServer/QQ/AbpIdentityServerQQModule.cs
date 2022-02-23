using LINGYUN.Abp.Tencent.QQ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityServer.QQ;

[DependsOn(typeof(AbpTencentQQModule))]
[DependsOn(typeof(AbpIdentityServerDomainModule))]
public class AbpIdentityServerQQModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services
            .AddAuthentication()
            .AddQQConnect();
    }
}