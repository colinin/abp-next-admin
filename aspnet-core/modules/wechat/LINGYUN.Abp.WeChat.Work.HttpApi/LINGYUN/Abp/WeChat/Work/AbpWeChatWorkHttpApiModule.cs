using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpWeChatWorkHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpWeChatWorkHttpApiModule).Assembly);
        });

        //PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        //{
        //    options.AddAssemblyResource(
        //        typeof(AbpTextTemplatingResource),
        //        typeof(AbpWeChatWorkApplicationContractsModule).Assembly);
        //});
    }
}
