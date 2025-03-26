using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Tencent;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class TencentOssContainerServiceCollectionExtensions
{
    public static IServiceCollection AddMinioContainer(this IServiceCollection services)
    {
        services.AddTransient<IOssContainerFactory, TencentOssContainerFactory>();

        services.AddTransient<IOssObjectExpireor>(provider =>
            provider
                .GetRequiredService<IOssContainerFactory>()
                .Create()
                .As<TencentOssContainer>());

        return services;
    }
}
