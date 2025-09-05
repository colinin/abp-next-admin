using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpAccountWeChatWorkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(WeChatWorkResource), typeof(AbpAccountWeChatWorkModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWeChatWorkModule).Assembly);
        });
    }
}
