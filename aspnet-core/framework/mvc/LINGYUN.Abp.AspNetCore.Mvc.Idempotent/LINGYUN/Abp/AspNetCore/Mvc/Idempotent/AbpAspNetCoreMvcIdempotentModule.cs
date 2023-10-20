using LINGYUN.Abp.Idempotent;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent;

[DependsOn(
    typeof(AbpIdempotentModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpAspNetCoreMvcIdempotentModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<MvcOptions>(options =>
        {
            options.Filters.AddService(typeof(AbpIdempotentActionFilter));
        });
    }
}
