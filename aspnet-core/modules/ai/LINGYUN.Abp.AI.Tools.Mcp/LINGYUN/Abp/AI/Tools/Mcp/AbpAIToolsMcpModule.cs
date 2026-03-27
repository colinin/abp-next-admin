using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AI.Tools.Mcp;

[DependsOn(
    typeof(AbpAIToolsModule),
    typeof(AbpHttpClientModule))]
public class AbpAIToolsMcpModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMcpAIToolClient();

        Configure<AbpAIToolsOptions>(options =>
        {
            // Mcp工具
            options.AIToolProviders.Add<McpAIToolProvider>();
        });
    }
}
