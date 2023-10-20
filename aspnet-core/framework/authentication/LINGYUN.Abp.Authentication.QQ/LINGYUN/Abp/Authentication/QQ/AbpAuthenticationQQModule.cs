using LINGYUN.Abp.Tencent.QQ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Authentication.QQ;

[DependsOn(typeof(AbpTencentQQModule))]
public class AbpAuthenticationQQModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services
            .AddAuthentication()
            .AddQQConnect();
    }
}