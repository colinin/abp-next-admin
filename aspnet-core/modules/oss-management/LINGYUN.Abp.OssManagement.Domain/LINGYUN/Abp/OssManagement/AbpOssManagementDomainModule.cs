using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpOssManagementDomainSharedModule),
        typeof(AbpDddDomainModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpFeaturesLimitValidationModule)
        )]
    public class AbpOssManagementDomainModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // TODO: 是否有必要自动创建容器
            var ossOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpOssManagementOptions>>().Value;
            var ossFactory = context.ServiceProvider.GetRequiredService<IOssContainerFactory>();
            var ossContainer = ossFactory.Create();

            foreach (var bucket in ossOptions.StaticBuckets)
            {
                _ = ossContainer.CreateIfNotExistsAsync(bucket);
            }
        }
    }
}
