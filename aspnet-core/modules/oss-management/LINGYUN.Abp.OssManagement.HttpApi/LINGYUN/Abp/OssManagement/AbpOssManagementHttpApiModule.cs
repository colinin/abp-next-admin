using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.AspNetCore.Mvc.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.Localization;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpOssManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class AbpOssManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpOssManagementHttpApiModule).Assembly);
            });

            PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(AbpOssManagementResource),
                    typeof(AbpOssManagementApplicationContractsModule).Assembly);
            });
        }

        //public override void ConfigureServices(ServiceConfigurationContext context)
        //{
        //    Configure<AbpLocalizationOptions>(options =>
        //    {
        //        options.Resources
        //            .Get<AbpOssManagementResource>()
        //            .AddBaseTypes(
        //                typeof(AbpAuthorizationResource),
        //                typeof(AbpValidationResource)
        //            );
        //    });
        //}
    }
}
