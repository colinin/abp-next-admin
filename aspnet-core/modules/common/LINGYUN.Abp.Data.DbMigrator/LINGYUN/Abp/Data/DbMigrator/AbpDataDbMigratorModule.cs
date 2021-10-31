using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Data.DbMigrator
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpDataDbMigratorModule : AbpModule
    {

    }
}
