using PackageName.CompanyName.ProjectName.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using LINGYUN.Abp.Dynamic.Queryable;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(ProjectNameApplicationContractsModule),
    typeof(AbpDynamicQueryableHttpApiModule))]
public class ProjectNameHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectNameHttpApiModule).Assembly);
        });

        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(ProjectNameResource),
                typeof(ProjectNameApplicationContractsModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ProjectNameResource>()
                .AddBaseTypes(typeof(AbpValidationResource));
        });
    }
}
