using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Official;

[DependsOn(
    typeof(AbpWeChatOfficialApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpWeChatOfficialHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpWeChatOfficialHttpApiModule).Assembly);
        });
    }
}
