using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement.Authorization;

public class WebhooksManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(WebhooksManagementPermissions.GroupName, L("Permission:WebhooksManagement"));

        var subscription = group.AddPermission(
            WebhooksManagementPermissions.WebhookSubscription.Default,
            L("Permission:Subscriptions"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Create,
            L("Permission:Create"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Update,
            L("Permission:Update"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);
        subscription.AddChild(
            WebhooksManagementPermissions.WebhookSubscription.Delete,
            L("Permission:Delete"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);

        var sendAttempts = group.AddPermission(
            WebhooksManagementPermissions.WebhooksSendAttempts.Default,
            L("Permission:SendAttempts"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);
        sendAttempts.AddChild(
            WebhooksManagementPermissions.WebhooksSendAttempts.Resend,
            L("Permission:Resend"),
            MultiTenancySides.Host)
            .WithProviders(ClientPermissionValueProvider.ProviderName);

        group.AddPermission(
            WebhooksManagementPermissions.Publish,
            L("Permission:Publish"))
            .WithProviders(ClientPermissionValueProvider.ProviderName);

        group.AddPermission(
            WebhooksManagementPermissions.ManageSettings,
            L("Permission:ManageSettings"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WebhooksManagementResource>(name);
    }
}
