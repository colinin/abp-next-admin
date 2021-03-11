using LINGYUN.Abp.Features.LimitValidation;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpFeaturesLimitValidationModule),
        typeof(AbpOssManagementDomainSharedModule)
        )]
    public class AbpOssManagementDomainModule : AbpModule
    {
    }
}
