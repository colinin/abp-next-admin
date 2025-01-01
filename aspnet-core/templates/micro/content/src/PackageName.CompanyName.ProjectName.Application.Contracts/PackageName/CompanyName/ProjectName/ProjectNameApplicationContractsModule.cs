using LINGYUN.Abp.Dynamic.Queryable;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpDynamicQueryableApplicationContractsModule),
    typeof(ProjectNameDomainSharedModule))]
public class ProjectNameApplicationContractsModule : AbpModule
{
}
