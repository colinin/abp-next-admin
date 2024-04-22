using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dynamic.Queryable;

[DependsOn(
    typeof(AbpDynamicQueryableApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpDynamicQueryableHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        //PreConfigure<IMvcBuilder>(mvcBuilder =>
        //{
        //    mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpDynamicQueryableHttpApiModule).Assembly);
        //});
    }
}