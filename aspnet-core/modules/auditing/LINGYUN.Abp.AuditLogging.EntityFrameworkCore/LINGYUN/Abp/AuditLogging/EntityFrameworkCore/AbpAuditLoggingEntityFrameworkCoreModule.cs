using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule),
        typeof(Volo.Abp.AuditLogging.EntityFrameworkCore.AbpAuditLoggingEntityFrameworkCoreModule))]
    [DependsOn(
        typeof(AbpAuditLoggingModule),
        typeof(AbpAutoMapperModule))]
    public class AbpAuditLoggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAuditingMapperProfile>(validate: true);
            });
        }
    }
}
