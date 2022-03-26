using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WebhooksManagement.Webhooks;

public class WebhooksDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        context.Add(
            new WebhookDefinition(
                WebhooksNames.CheckConnect,
                L("DisplayName:CheckConnect"),
                L("Description:CheckConnect")));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<WebhooksManagementResource>(name);
    }
}
