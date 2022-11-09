using LINGYUN.Abp.IdGenerator.Snowflake;
using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdGenerator;

[DependsOn(
    typeof(AbpIdGeneratorModule),
    typeof(AbpTestsBaseModule))]
public class AbpIdGeneratorModuleTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<SnowflakeIdOptions>(options =>
        {
            options.WorkerId = 30010 - 30000;
            options.WorkerIdBits = 5;
            options.DatacenterId = 1;
        });
    }
}
