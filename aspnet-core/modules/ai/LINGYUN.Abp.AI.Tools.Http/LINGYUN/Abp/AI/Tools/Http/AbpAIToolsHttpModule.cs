using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AI.Tools.Http.ApplicationConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AI.Tools.Http;

[DependsOn(
    typeof(AbpAIToolsModule),
    typeof(AbpHttpClientModule),
    typeof(AbpAspNetCoreMvcContractsModule))]
public class AbpAIToolsHttpModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAIToolsApplicationConfigurationOptions>(context.Configuration.GetSection("AITools:Http:ApplicationConfiguration"));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAIToolsHttpModule>();
        });

        Configure<AbpAIToolsOptions>(options =>
        {
            // Http工具
            options.AIToolProviders.Add<HttpAIToolProvider>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpAIResource>()
                .AddVirtualJson("/LINGYUN/Abp/AI/Tools/Http/Localization/Resources");
        });

        context.Services.AddHttpAIToolClient();
    }
}
