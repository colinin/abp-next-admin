using Volo.Abp.BackgroundJobs;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks;

[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpFeaturesModule))]
[DependsOn(typeof(AbpGuidsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class AbpWebhooksModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }
}
