using LINGYUN.Abp.AuditLogging;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Auditing
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAuditLoggingModule),
        typeof(AbpAuditingApplicationContractsModule))]
    public class AbpAuditingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpAuditingApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAuditingMapperProfile>(validate: true);
            });
        }
    }
}
