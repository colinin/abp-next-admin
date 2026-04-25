using LINGYUN.Abp.AI.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AI.Tools.Mcp;

[DependsOn(
    typeof(AbpAIToolsModule),
    typeof(AbpHttpClientModule))]
public class AbpAIToolsMcpModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMcpAIToolClient();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAIToolsMcpModule>();
        });

        Configure<AbpAIToolsOptions>(options =>
        {
            // Mcp工具
            options.AIToolProviders.Add<McpAIToolProvider>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpAIResource>()
                .AddVirtualJson("/LINGYUN/Abp/AI/Tools/Mcp/Localization/Resources");
        });
    }
}
