using LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Filters;
using LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Localization;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ProxyScripting;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper
{
    [DependsOn(
        typeof(AbpWrapperModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpAspNetCoreMvcWrapperModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcWrapperModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpMvcWrapperResource>("en")
                    .AddVirtualJson("/LINGYUN/Abp/AspNetCore/Mvc/Wrapper/Localization/Resources");
            });

            Configure<MvcOptions>(mvcOptions =>
            {
                // Wrap Result Filter
                mvcOptions.Filters.AddService(typeof(AbpWrapResultFilter));
            });

            Configure<AbpWrapperOptions>(options =>
            {
                // 即使重写端点也不包装返回结果
                // api/abp/api-definition
                options.IgnoreReturnTypes.Add<ApplicationApiDescriptionModel>();
                // api/abp/application-configuration
                options.IgnoreReturnTypes.Add<ApplicationConfigurationDto>();
                // 
                options.IgnoreReturnTypes.Add<IRemoteStreamContent>();
                // Abp/ServiceProxyScript
                options.IgnoreControllers.Add<AbpServiceProxyScriptController>();

                // 官方模块不包装结果
                options.IgnoreNamespaces.Add("Volo.Abp");

                // 返回本地化的 404 错误消息
                options.MessageWithEmptyResult = (serviceProvider) =>
                {
                    var localizer = serviceProvider.GetRequiredService<IStringLocalizer<AbpMvcWrapperResource>>();
                    return localizer["Wrapper:NotFound"];
                };
            });
        }
    }
}
