using LINGYUN.Abp.WorkflowManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.WorkflowManagement
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(WorkflowManagementApplicationContractsModule))]
    public class WorkflowManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(WorkflowManagementHttpApiModule).Assembly);
            });

            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                        typeof(WorkflowManagementResource),
                        typeof(WorkflowManagementApplicationContractsModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WorkflowManagementResource>()
                    .AddBaseTypes(typeof(AbpValidationResource));
            });
        }
    }
}
