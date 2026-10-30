using LINGYUN.Abp.ElsaNext.ApprovalFlow.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpElsaNextApprovalFlowApplicationContractsModule),
    typeof(AbpElsaNextApprovalFlowApplicationModule))]
public class AbpElsaNextApprovalFlowHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpElsaNextApprovalFlowHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(ElsaNextApprovalFlowResource),
                typeof(AbpElsaNextApprovalFlowApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextApprovalFlowResource>()
                .AddBaseTypes(typeof(AbpValidationResource));
        });
    }
}
