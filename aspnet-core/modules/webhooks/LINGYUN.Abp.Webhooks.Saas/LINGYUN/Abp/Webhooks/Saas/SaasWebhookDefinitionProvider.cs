using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks.Saas;

public class SaasWebhookDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        var saasGroup = context.AddGroup(
            SaasWebhookNames.GroupName,
            L("Webhooks:Saas"));

        saasGroup.AddWebhooks(CreateEditionWebhooks());
        saasGroup.AddWebhooks(CreateTenantWebhooks());
    }

    protected virtual WebhookDefinition[] CreateEditionWebhooks()
    {
        return new[]
        {
            new WebhookDefinition(
                SaasWebhookNames.Edition.Create,
                L("Webhooks:CreateEdition"),
                L("Webhooks:CreateEditionDesc")),
            new WebhookDefinition(
                SaasWebhookNames.Edition.Update,
                L("Webhooks:UpdateEdition"),
                L("Webhooks:UpdateEditionDesc")),
            new WebhookDefinition(
                SaasWebhookNames.Edition.Delete,
                L("Webhooks:DeleteEdition"),
                L("Webhooks:DeleteEditionDesc")),
        };
    }

    protected virtual WebhookDefinition[] CreateTenantWebhooks()
    {
        return new[]
        {
            new WebhookDefinition(
                SaasWebhookNames.Tenant.Create,
                L("Webhooks:CreateTenant"),
                L("Webhooks:CreateTenantDesc")),
            new WebhookDefinition(
                SaasWebhookNames.Tenant.Update,
                L("Webhooks:UpdateTenant"),
                L("Webhooks:UpdateTenantDesc")),
            new WebhookDefinition(
                SaasWebhookNames.Tenant.Delete,
                L("Webhooks:DeleteTenant"),
                L("Webhooks:DeleteTenantDesc")),
        };
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSaasResource>(name);
    }
}
