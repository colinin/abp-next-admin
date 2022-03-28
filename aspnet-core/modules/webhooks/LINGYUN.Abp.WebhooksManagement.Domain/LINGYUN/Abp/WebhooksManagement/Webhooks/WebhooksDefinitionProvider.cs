using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.WebhooksManagement.Webhooks;

public class WebhooksDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        var testsGroup = context.AddGroup(
            WebhooksNames.GroupName,
            L("Webhooks:Tests"));

        testsGroup.AddWebhooks(
            new WebhookDefinition(
                WebhooksNames.CheckConnect,
                L("Webhooks:CheckConnect"),
                L("Webhooks:CheckConnectDesc")));
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<WebhooksManagementResource>(name);
    }
}
