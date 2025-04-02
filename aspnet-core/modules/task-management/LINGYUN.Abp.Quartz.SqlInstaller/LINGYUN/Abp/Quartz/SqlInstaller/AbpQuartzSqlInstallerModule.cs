using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

[DependsOn(typeof(AbpQuartzModule))]
public class AbpQuartzSqlInstallerModule : AbpModule
{
}
