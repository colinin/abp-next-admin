using LINGYUN.Abp.AspNetCore.Mvc.GlobalFeatures;
using LINGYUN.Abp.AspNetCore.Mvc.Localization;
using LINGYUN.Abp.AspNetCore.Mvc.Results;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Wrapper;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Content;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcWrapperModule),
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreMvcTestModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(MvcTestResource),
                    typeof(AbpAspNetCoreMvcTestModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            OneTimeRunner.Run(() =>
            {
                GlobalFeatureManager.Instance.Modules.GetOrAdd(AbpAspNetCoreMvcTestFeatures.ModuleName,
                    () => new AbpAspNetCoreMvcTestFeatures(GlobalFeatureManager.Instance))
                    .EnableAll();
            });

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

            Configure<AbpWrapperOptions>(options =>
            {
                options.IsEnabled = true;

                // 测试先清空
                options.IgnoreControllers.Clear();
                options.IgnoreExceptions.Clear();
                options.IgnoreNamespaces.Clear();
                options.IgnorePrefixUrls.Clear();
                options.IgnoredInterfaces.Clear();
                options.IgnoreReturnTypes.Clear();

                // api/abp/api-definition
                options.IgnoreControllers.Add<AbpApiDefinitionController>();
                options.IgnoredInterfaces.Add<IWrapDisabled>();
                // api/abp/application-configuration
                options.IgnoreReturnTypes.Add<ApplicationConfigurationDto>();
                options.IgnoreReturnTypes.Add<IRemoteStreamContent>();

                options.IgnorePrefixUrls.Add("/api/dont/wrap-result-test");

                options.IgnoreExceptions.Add<HasDbException>();
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MvcTestResource>("en")
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
                options.ErrorCodeNamespaceMappings.Add("Test", typeof(MvcTestResource));
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
            app.UseAuthorization();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}
