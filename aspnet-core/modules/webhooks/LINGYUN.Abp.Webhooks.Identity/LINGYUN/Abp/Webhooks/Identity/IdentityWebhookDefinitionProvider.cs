using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Webhooks.Identity;

public class IdentityWebhookDefinitionProvider : WebhookDefinitionProvider
{
    public override void Define(IWebhookDefinitionContext context)
    {
        var identityGroup = context.AddGroup(
            IdentityWebhookNames.GroupName,
            L("Webhooks:Identity"));

        identityGroup.AddWebhooks(CreateIdentityRoleWebhooks());
        identityGroup.AddWebhooks(CreateIdentityUserWebhooks());
    }

    protected virtual WebhookDefinition[] CreateIdentityRoleWebhooks()
    {
        return new[]
        {
            new WebhookDefinition(
                IdentityWebhookNames.IdentityRole.Create,
                L("Webhooks:CreateRole"),
                L("Webhooks:CreateRoleDesc")),
            new WebhookDefinition(
                IdentityWebhookNames.IdentityRole.Update,
                L("Webhooks:UpdateRole"),
                L("Webhooks:UpdateRoleDesc")),
            new WebhookDefinition(
                IdentityWebhookNames.IdentityRole.Delete,
                L("Webhooks:DeleteRole"),
                L("Webhooks:DeleteRoleDesc")),
            new WebhookDefinition(
                IdentityWebhookNames.IdentityRole.ChangeName,
                L("Webhooks:ChangeRoleName"),
                L("Webhooks:ChangeRoleNameDesc")),
        };
    }

    protected virtual WebhookDefinition[] CreateIdentityUserWebhooks()
    {
        return new[]
        {
            new WebhookDefinition(
                IdentityWebhookNames.IdentityUser.Create,
                L("Webhooks:CreateUser"),
                L("Webhooks:CreateUserDesc")),
            new WebhookDefinition(
                IdentityWebhookNames.IdentityUser.Update,
                L("Webhooks:UpdateUser"),
                L("Webhooks:UpdateUserDesc")),
            new WebhookDefinition(
                IdentityWebhookNames.IdentityUser.Delete,
                L("Webhooks:DeleteUser"),
                L("Webhooks:DeleteUserDesc")),
        };
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
