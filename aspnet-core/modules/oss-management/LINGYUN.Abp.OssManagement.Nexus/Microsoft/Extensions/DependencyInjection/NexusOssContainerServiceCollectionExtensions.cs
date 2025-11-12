using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Nexus;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class NexusOssContainerServiceCollectionExtensions
{
    public static IServiceCollection AddNexusContainer(this IServiceCollection services)
    {
        services.AddTransient<IOssContainerFactory, NexusOssContainerFactory>();

        services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<NexusOssContainer>());

        return services;
    }
}
