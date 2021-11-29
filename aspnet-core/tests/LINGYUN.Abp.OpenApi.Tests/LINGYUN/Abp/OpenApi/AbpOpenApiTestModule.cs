using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.OpenApi.Authorization;
using LINGYUN.Abp.OpenApi.Localization;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using OpenApi;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.OpenApi
{
    [DependsOn(
        typeof(AbpOpenApiAuthorizationModule),
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpAspNetCoreMvcWrapperModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpOpenApiTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(OpenApiResource),
                    typeof(AbpOpenApiTestModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 测试环境不需要主机地址
            context.Services.AddHttpClient("opensdk");
            context.Services.AddSingleton<IClientProxy, ClientProxy>();

            context.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = "Bearer";
                options.DefaultForbidScheme = "Cookie";
            }).AddCookie("Cookie").AddJwtBearer("Bearer", _ => { });

            context.Services.AddAuthorization(options =>
            {
            });

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpOpenApiTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OpenApiResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource),
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/LINGYUN/Abp/AspNetCore/Mvc/Localization/Resources");

                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.RootDirectory = "/LINGYUN/Abp/AspNetCore/Mvc";
            });

            Configure<AbpClaimsMapOptions>(options =>
            {

            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.ErrorCodeNamespaceMappings.Add("Test", typeof(OpenApiResource));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseAbpRequestLocalization();
            app.UseAbpSecurityHeaders();
            app.UseRouting();
            app.UseAbpClaimsMap();
            app.UseAuthentication();
            app.UseOpenApiAuthorization();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}
