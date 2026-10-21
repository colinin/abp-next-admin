using Microsoft.Extensions.Configuration;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.MicroService.AllInOne.DbMigrator;
public partial class SingleDbMigratorModule
{
    private void ConfigureTiming(IConfiguration configuration)
    {
        Configure<AbpClockOptions>(options =>
        {
            configuration.GetSection("Clock").Bind(options);
        });
    }
}
