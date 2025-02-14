using LINGYUN.Abp.UI.Navigation.VueVbenAdmin;
using PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DatabaseManagementName;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName.AIO.DbMigrator;

[DependsOn(
    typeof(AbpUINavigationVueVbenAdminModule),
    typeof(SingleMigrationsEntityFrameworkCoreDatabaseManagementNameModule),
    typeof(AbpAutofacModule)
    )]
public class SingleDbMigratorModule : AbpModule
{
}
