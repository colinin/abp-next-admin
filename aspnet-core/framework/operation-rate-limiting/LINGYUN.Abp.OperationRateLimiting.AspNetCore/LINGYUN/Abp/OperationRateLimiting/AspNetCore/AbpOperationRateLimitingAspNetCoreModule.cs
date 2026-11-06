using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting.AspNetCore;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpOperationRateLimitingModule))]
public class AbpOperationRateLimitingAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<AbpOperationRateLimitingActionFilter>();

        Configure<MvcOptions>(options =>
        {
            options.Filters.AddService(typeof(AbpOperationRateLimitingActionFilter));
        });
    }
}
