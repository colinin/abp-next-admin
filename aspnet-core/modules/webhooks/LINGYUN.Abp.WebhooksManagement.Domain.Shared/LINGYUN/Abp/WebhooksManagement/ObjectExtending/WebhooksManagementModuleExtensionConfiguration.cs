using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace LINGYUN.Abp.WebhooksManagement.ObjectExtending;

public class WebhooksManagementModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public WebhooksManagementModuleExtensionConfiguration ConfigureWebhooksManagement(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this
            .ConfigureEntity(WebhooksManagementModuleExtensionConsts.EntityNames.WebhookEvent, configureAction)
            .ConfigureEntity(WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSendAttempt, configureAction)
            .ConfigureEntity(WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSubscription, configureAction);
    }
}
