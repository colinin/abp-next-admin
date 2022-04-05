using LINGYUN.Abp.WebhooksManagement;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks.ClientProxies;

[DependsOn(typeof(AbpWebhooksModule))]
[DependsOn(typeof(WebhooksManagementHttpApiClientModule))]
public class AbpWebHooksClientProxiesModule : AbpModule
{
}
