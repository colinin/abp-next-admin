using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Auditing
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpAuditLoggingDomainModule),
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
