using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using VoloAbpPermissionManagementEntityFrameworkCoreModule = Volo.Abp.PermissionManagement.EntityFrameworkCore.AbpPermissionManagementEntityFrameworkCoreModule;

namespace LINGYUN.Abp.PermissionManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpPermissionManagementDomainModule),
    typeof(VoloAbpPermissionManagementEntityFrameworkCoreModule))]
public class AbpPermissionManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PermissionManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IPermissionManagementDbContext>();

            options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
        });
    }
}
