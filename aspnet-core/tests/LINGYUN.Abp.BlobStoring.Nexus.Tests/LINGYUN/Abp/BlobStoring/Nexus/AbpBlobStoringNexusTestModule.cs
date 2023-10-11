using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Nexus;

[DependsOn(
    typeof(AbpBlobStoringNexusModule),
    typeof(AbpTestsBaseModule))]
public class AbpBlobStoringNexusTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\BlobStoring\Nexus",
            EnvironmentName = "Test"
        };

        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseNexus(nexus =>
                {
                    nexus.BasePath = configuration[NexusBlobProviderConfigurationNames.BasePath];
                    nexus.Repository = configuration[NexusBlobProviderConfigurationNames.Repository];
                });
            });
        });
    }
}
