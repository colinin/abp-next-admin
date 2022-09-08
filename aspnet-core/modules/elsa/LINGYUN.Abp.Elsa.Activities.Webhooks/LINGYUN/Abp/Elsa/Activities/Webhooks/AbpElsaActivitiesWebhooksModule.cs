using LINGYUN.Abp.Webhooks;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpWebhooksModule))]
public class AbpElsaActivitiesWebhooksModule : AbpModule
{
}
