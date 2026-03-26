using LINGYUN.Abp.AI.Localization;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
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
            options.ChatClientBuildActions.Add(async (_, sp, builder) =>
            {
                var aiToolFactory = sp.GetRequiredService<IAIToolFactory>();
                var aiTools = await aiToolFactory.CreateAllTools();

                builder.ConfigureOptions(ai =>
                {
                    ai.ToolMode = ChatToolMode.Auto;
                    ai.AllowMultipleToolCalls = true;

                    ai.Tools ??= [];

                    foreach (var aiTool in aiTools)
                    {
                        ai.Tools.Add(aiTool);
                    }
                });
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
