using LINGYUN.Abp.Dynamic.Queryable;
using LINGYUN.Abp.Exporter;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpExporterApplicationContractsModule),
    typeof(AbpDynamicQueryableApplicationContractsModule),
    typeof(ProjectNameDomainSharedModule))]
public class ProjectNameApplicationContractsModule : AbpModule
{
}
