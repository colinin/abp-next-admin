using LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpElsaEntityFrameworkCoreModule : AbpModule
{
    public async override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // 实现 IElsaStoreMigrator 接口自动初始化数据库
        await context.ServiceProvider
            .GetService<IElsaDataBaseInstaller>()
            ?.InstallAsync();
    }
}
