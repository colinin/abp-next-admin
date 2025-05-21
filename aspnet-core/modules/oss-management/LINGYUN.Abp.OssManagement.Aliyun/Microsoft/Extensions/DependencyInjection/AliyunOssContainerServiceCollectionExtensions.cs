using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Aliyun;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class AliyunOssContainerServiceCollectionExtensions
{
    public static IServiceCollection AddAliyunContainer(this IServiceCollection services)
    {
        services.AddTransient<IOssContainerFactory, AliyunOssContainerFactory>();

        services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<AliyunOssContainer>());

        return services;
    }
}
