using LY.MicroService.RealtimeMessage.DbMigrator.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.RealtimeMessage.DbMigrator;
public partial class RealtimeMessageDbMigratorModule
{
    private void ConfigureDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<RealtimeMessageMigrationsDbContext>();

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
