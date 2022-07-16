using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(Volo.Abp.Account.AbpAccountHttpApiModule))]
    public class AbpAccountHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountApplicationContractsModule).Assembly);
                // 原生的在Web项目指定,不没有引用Web项目的情况下需要它
                options.AddAssemblyResource(typeof(AccountResource), typeof(Volo.Abp.Account.AbpAccountApplicationContractsModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountHttpApiModule).Assembly);
            });
        }
    }
}
