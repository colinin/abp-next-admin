using LINGYUN.Abp.TaskManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.TaskManagement;

[DependsOn(typeof(TaskManagementApplicationContractsModule))]
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class TaskManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(TaskManagementHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                    typeof(TaskManagementResource),
                    typeof(TaskManagementApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TaskManagementResource>()
                .AddBaseTypes(typeof(AbpValidationResource));
        });
    }
}
