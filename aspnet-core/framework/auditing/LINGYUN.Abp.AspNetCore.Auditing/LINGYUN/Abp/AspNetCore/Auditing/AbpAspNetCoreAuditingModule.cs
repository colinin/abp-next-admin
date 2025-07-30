using Volo.Abp.AspNetCore;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.Auditing;

[DependsOn(typeof(AbpAspNetCoreModule))]
public class AbpAspNetCoreAuditingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreRecordHeaderAuditLogContributor());
        });
    }
}
