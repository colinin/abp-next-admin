using LINGYUN.Abp.AI.Localization;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AI.Tools;

[DependsOn(typeof(AbpAICoreModule))]
public class AbpAIToolsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAIToolsModule>();
        });

        Configure<AbpAIToolsOptions>(options =>
        {
            // 自定义函数工具
            options.AIToolProviders.Add<FunctionAIToolProvider>();
        });

        Configure<AbpAICoreOptions>(options =>
        {
            options.ChatClientBuildActions.Add(async (workspace, sp, builder) =>
            {
                var workspaceAIToolFinder = sp.GetRequiredService<IWorkspaceAIToolFinder>();
                var workspaceAITools = await workspaceAIToolFinder.GetToolsAsync(workspace);

                return builder
                    .ConfigureOptions(config =>
                    {
                        config.ToolMode = ChatToolMode.Auto;
                        config.AllowMultipleToolCalls = true;

                        // 添加发现的工具
                        config.Tools = workspaceAITools;
                    })
                    // 启用以支持函数式工具
                    .UseFunctionInvocation();
            });
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpAIResource>()
                .AddVirtualJson("/LINGYUN/Abp/AI/Tools/Localization/Resources");
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IAIToolDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpAIToolsOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
