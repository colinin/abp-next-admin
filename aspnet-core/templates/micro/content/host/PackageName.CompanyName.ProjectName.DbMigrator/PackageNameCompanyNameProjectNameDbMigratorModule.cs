using PackageName.CompanyName.ProjectName.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ProjectNameEntityFrameworkCoreModule)
    )]
public class PackageNameCompanyNameProjectNameDbMigratorModule : AbpModule
{
}
