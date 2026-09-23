using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using VoloAbpAuditLoggingEntityFrameworkCoreModule = Volo.Abp.AuditLogging.EntityFrameworkCore.AbpAuditLoggingEntityFrameworkCoreModule;
using VoloAbpIdentityEntityFrameworkCoreModule = Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule;
using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[DependsOn(
    typeof(VoloAbpIdentityEntityFrameworkCoreModule),
    typeof(VoloAbpAuditLoggingEntityFrameworkCoreModule))]
[DependsOn(
    typeof(AbpAuditLoggingModule),
    typeof(AbpMapperlyModule))]
public class AbpAuditLoggingEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule>();

        context.Services.AddAbpDbContext<AbpAuditLoggingDbContext>(options =>
        {
            options.AddRepository<VoloAuditLog, EfCoreAuditLogRepository>();
        });
    }
}
