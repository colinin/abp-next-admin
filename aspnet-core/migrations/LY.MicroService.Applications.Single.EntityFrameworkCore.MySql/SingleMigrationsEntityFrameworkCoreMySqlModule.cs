using LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.MySql;

[DependsOn(
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpElsaEntityFrameworkCoreMySqlModule),
    typeof(SingleMigrationsEntityFrameworkCoreModule)
    )]
public class SingleMigrationsEntityFrameworkCoreMySqlModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SingleMigrationsDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
