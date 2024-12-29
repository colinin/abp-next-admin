using Microsoft.Extensions.Configuration;
using Volo.Abp.Timing;

namespace PackageName.CompanyName.ProjectName.AIO.DbMigrator;
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
