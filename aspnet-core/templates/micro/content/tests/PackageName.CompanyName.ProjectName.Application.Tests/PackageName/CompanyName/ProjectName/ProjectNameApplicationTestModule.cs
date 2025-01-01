using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(ProjectNameDomainTestModule),
    typeof(ProjectNameApplicationModule)
    )]
public class ProjectNameApplicationTestModule : AbpModule
{
}
