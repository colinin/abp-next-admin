﻿using LINGYUN.Abp.Dynamic.Queryable;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpDynamicQueryableApplicationContractsModule),
    typeof(ProjectNameDomainSharedModule))]
public class ProjectNameApplicationContractsModule : AbpModule
{
}
