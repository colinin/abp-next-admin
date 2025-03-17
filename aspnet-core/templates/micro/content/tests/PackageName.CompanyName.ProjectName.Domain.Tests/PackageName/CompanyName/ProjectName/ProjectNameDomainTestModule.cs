using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(ProjectNameTestBaseModule),
    typeof(ProjectNameDomainModule)
    )]
public class ProjectNameDomainTestModule : AbpModule
{
}
