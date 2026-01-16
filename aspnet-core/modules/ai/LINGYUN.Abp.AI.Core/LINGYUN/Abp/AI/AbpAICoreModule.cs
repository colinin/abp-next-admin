using LINGYUN.Abp.AI.Internal;
using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.AI;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AI;

[DependsOn(
    typeof(AbpAIModule),
    typeof(AbpLocalizationModule))]
public class AbpAICoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAICoreModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpAIResource>()
                .AddVirtualJson("/LINGYUN/Abp/AI/Localization/Resources");
        });

        Configure<AbpAICoreOptions>(options =>
        {
            options.ChatClientProviders.Add<OpenAIChatClientProvider>();
            options.ChatClientProviders.Add<DeepSeekChatClientProvider>();

            options.KernelProviders.Add<OpenAIKernelProvider>();
            options.KernelProviders.Add<DeepSeekKernelProvider>();
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AbpAIErrorCodes.Namespace, typeof(AbpAIResource));
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IWorkspaceDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpAICoreOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
